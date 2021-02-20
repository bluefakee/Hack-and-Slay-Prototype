using UnityEngine;

// PlayerMovement writen by bluefake
// In Update all regions have a explanation of what the code there does
// If you experience bugs and things that you think shouldnt happen or just need help, just dm me on discord (bluefake#3507)

// Work In Progress:
// Eventsystem for Animations
// After falling of a cliff, you can still jump (like in Celeste)

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    #region Groundcheck Fields

    [Header("Groundchecks"), SerializeField]
    private Transform feet;

    [SerializeField]
    private Transform head, left, right;

    [SerializeField]
    private Vector2 feetSize, headSize, leftSize, rightSize;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField, Tooltip("Shows the hitboxes of the checks")]
    private bool debugChecks;

    private bool isGrounded;            // Is the player on Ground?
    protected bool isCeilingAbove;      // Is the player hitting a Ceiling?
    protected bool isWallLeft;          // Is the player touching a wall left
    protected bool isWallRight;         // Is the player touching a wall right?

    #endregion

    #region Movement Fields

    [Header("Movement"), SerializeField, Range(0f, 30f), Tooltip("How fast the maximum velocity of the player is")]
    private float moveSpeed;
    [SerializeField, Range(0f, 1f), Tooltip("How long does it take to reach max speed")]
    private float accTime;
    [SerializeField, Range(0f, 1f), Tooltip("How long does it take to stop moving when you where at max speed")]
    private float deccTime;

    private float move;         // saves the Inputs
    private float timer;        // a counter for the acc and decc of the object, when it reaches 1 or -1, the max velocity is reached

    #endregion

    #region Jump Fields

    [Header("Jumping"), SerializeField, Range(0f, 20f), Tooltip("How fast is the velocity when jumping")]
    private float jumpForce;

    [SerializeField, Range(0f, 1f), Tooltip("The durration the vertical velocity of the jump is applied")]
    private float jumpDurr;

    [SerializeField, Range(0f, 1f), Tooltip("A multiplyer that gets applied to the acc and decc when the player is not grounded")]
    private float midAirSlow = 0.5f;

    private float jumpTimer;        // Counts the time down while the object jumps (both walljump and normal)

    private bool isJumping;         // has the Player started Jump()?

    #endregion

    #region WallSlide Fields

    [Header("Wallsliding"), SerializeField, Range(0f, 1f), Tooltip("The time the player needs to hold the other direction in order to get away from a wall")]
    private float unAttachDurr;

    [SerializeField, Range(0f, 1f), Tooltip("With what gets the grav of the player multiplyed when wallSliding")]
    private float gravMulti = 0.5f;

    private float unAttachTimer;    // Countdown until the sliding gets canceled
    private int direction;        // Saves where the player is jumping to

    private bool isSliding;         // if true, the player grabbed the next wall and needs to hold the other direction for a certain time
                                    // its recommendet to use SetSliding when changing this value

    // Sets the Sliding and automaticly sets the gravity
    private void SetSliding(bool isSliding) 
    {
        if (this.isSliding == isSliding)
            return;
    
        this.isSliding = isSliding;

        rb.gravityScale *= isSliding ? gravMulti : (1 / gravMulti);
    }

    #endregion

    #region WallJump Fields

    [Header("Walljumping"), SerializeField, Range(0f, 10f), Tooltip("How fast is the velocity when jumping of a wall")]
    private float wallJumpForce;

    [SerializeField, Range(0f, 1f), Tooltip("How long does the wallJump get executed (note that it cannot be canceled)")]
    private float wallJumpDurr;

    private bool isWallJumping;     // gets true instead of isJumping when Jump() called and on a wall

    #endregion

    #region Crouching Fields

    [Header("Crouching"), SerializeField, Range(0f, 1f), Tooltip("How much gets the player slowed when crouching")]
    private float crouchSlow;

    [SerializeField, Range(0f, 1f), Tooltip("With what gets the size multiplyed when enableing crouching")]
    private float crouchSizeMulti;

    [SerializeField, Range(0f, 10f), Tooltip("How much gets the drag increased when crouching midair (also on walls)")]
    private float crouchDrag;

    private bool isCrouching;       // Its recommended to use it similar to isSliding

    private void SetCrouching(bool isCrouching)
    {
        if (isCrouching == this.isCrouching)
            return;

        this.isCrouching = isCrouching;

        // Save the new ySize of the collider
        float ySize = coll.size.y * (isCrouching ? crouchSizeMulti : 1 / crouchSizeMulti);

        // Apply all changes to the collider
        coll.offset -= new Vector2(0, coll.size.y - ySize) * 0.5f;
        coll.size = new Vector2(coll.size.x, ySize);

        // Increase grav scale
        rb.gravityScale *= isCrouching ? crouchDrag : 1 / crouchDrag;

        // Reduce the move to simulate sliding
        move *= isCrouching ? crouchSlow : 1 / crouchSlow;
    }

    #endregion

    void OnDrawGizmos()
    {
        if(debugChecks)
        {
            Gizmos.DrawCube(feet.position, new Vector3(feetSize.x, feetSize.y, 0.1f));
            Gizmos.DrawCube(head.position, new Vector3(headSize.x, headSize.y, 0.1f));
            Gizmos.DrawCube(left.position, new Vector3(leftSize.x, leftSize.y, 0.1f));
            Gizmos.DrawCube(right.position, new Vector3(rightSize.x, rightSize.y, 0.1f));
        }
    }

    void Update()
    {
        #region Groundcheck

        // The groundcheck checks in a box of the size that you can set in the inspector if there is any ground
        isGrounded = Physics2D.OverlapBox(feet.position, feetSize, 0, whatIsGround);
        isCeilingAbove = Physics2D.OverlapBox(head.position, headSize, 0, whatIsGround);
        isWallLeft = Physics2D.OverlapBox(left.position, leftSize, 0, whatIsGround);
        isWallRight = Physics2D.OverlapBox(right.position, rightSize, 0, whatIsGround);

        #endregion

        #region Movement Calculation

        // The movement works like this:
        // The move represents the input of the player through SetMove()
        // The timer is a counter that counts from -1 to 1 and gets multiplyed with the moveSpeed in FixedUpdate
        // The timer slowly counts up or down depending of the move using accTime
        // If move is 0 or the timer is higher when the move, the timer slowly decreases using deccTime
        // If the player is not grounded, the acc and deccTime get longer depending on the midAirSlow (0.5 midAirSlow means double acc/deccTime)
        // Below the system prevents that the player can be stuck in walls when moving towards it

        if(move > 0 && timer <= move)
        {
            // the timer is not bigger as move and the player inputs right, timer increases intil it reaches move
            timer = Mathf.Clamp(timer + Time.deltaTime * (isGrounded ? 1 : midAirSlow) / accTime , -1, move);
        }
        else if (move < 0 && timer >= move)
        {
            // same like above, only for left and decreasing
            timer = Mathf.Clamp(timer - Time.deltaTime * (isGrounded ? 1 : midAirSlow) / accTime, move, 1);
        }
        else
        {
            // no input or the timer results in faster movement when the inputs allow -> the timer nears to 0
            timer = timer > 0 ? Mathf.Clamp(timer - Time.deltaTime * (isGrounded ? 1 : midAirSlow) / deccTime, 0, 1) :
                                Mathf.Clamp(timer + Time.deltaTime * (isGrounded ? 1 : midAirSlow) / deccTime, -1, 0);
        }

        // Prevent that the player gets stuck on walls
        if (timer > 0 && isWallRight) timer = 0;

        if (timer < 0 && isWallLeft) timer = 0;

        #endregion

        #region Jump Management

        // The jumping works like this:
        // isJumping and isWallJumping can both be set true by Jump() (of course if the player isGrounded or Sliding)
        // when is(Wall)Jumping gets set true, jumpTimer gets set to the (wall)JumpDurr and gets counted down
        // if it reaches zero, the is(Wall)Jumping gets set false (when wallJumping the timer gets set to the jumpdirection to keep the momentum)
        // Both gets canceled if a Ceiling is touched
        // When isJumping is true, the vertical velocity gets set to jumpForce
        // When isWallJumping is true, the vertical velocity gets set to wallJumpForce and the directon of the jump is saved and used instead the timer
        // (Im probably adding the ability to still be grounded if you run of a plattform like in celeste)

        if ((isJumping || isWallJumping) && jumpTimer > 0 && !isCeilingAbove)
        {
            // Count down the time the player is jumping up
            jumpTimer -= Time.deltaTime;
        }
        else if (isJumping)
        {
            // Cancel jump
            isJumping = false;
        }
        else if(isWallJumping)
        {
            // Cancel wallJump
            isWallJumping = false;
            timer = direction;
        }

        #endregion
    
        #region WallSliding

        // The Sliding works like this:
        // If you are not grounded and moving towards a wall, isSliding becomes true
        // It also becomes true if you wallJump at a wall what allows the player to spam Jump() when between 2 walls
        // Also the gravityScale of the rb gets multiplyed by gravMulti to simulate wallSliding and gets reverted when stopping to slide (see SetSliding())
        // If move goes away from the wall, the countdown unAttachTimer counts down
        // It gets reset to usAttachDurr when the move becomes 0 or moves towards a wall
        // After the countdown reaches zero, the player stops sliding on the wall and falls
        // This is done because it can feel bad if you instantly stop sliding when pressing away from the wall and can deny the player

        if (!isGrounded && !isSliding && (isMovingToWall(move) || (isMovingToWall(direction) && isWallJumping)))
        {
            // The player is moving against a wall while not grounded or wallJumps at a wall and starts sliding
            SetSliding(true);
        }

        // This is to simulate that the player needs to hold a short amount
        if (isSliding && (isWallLeft && move > 0 || isWallRight && move < 0))
        {
            // Count down until it reaches zero and resets the slide
            unAttachTimer -= Time.deltaTime;
            timer = 0;      // Im gonna stop you right there
        }
        else if (isSliding)
        {
            // Reset the Countdown (gets automaticly called when starting to slide since move would be towards a wall)
            unAttachTimer = unAttachDurr;
        }

        if ((isGrounded || unAttachTimer <= 0) && isSliding)
        {
            // Reset the slide
            SetSliding(false);
        }

        #endregion
    }

    void FixedUpdate()
    {
        // Change the velocity. The Sin results in a smooth curve (for both positive and negative for the x-Axis)
        // https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/ is a good article that has such formulas that you can use for smoother curves
        // If the player is jumping, the y velocity is set to jumpForce, else the velocity stays as it is
        // The Sin isnt used for jumping because this results in a weird feeling when jumping somehow
        rb.velocity = isWallJumping ? new Vector2(direction * moveSpeed, wallJumpForce) : 
            new Vector2(Mathf.Sin(timer * 0.5f * Mathf.PI) * moveSpeed, isJumping ? jumpForce : rb.velocity.y);
    }

    void Awake()
    {
        // Get referenzes
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        // Prevent divisions with 0
        if (accTime == 0) accTime = 0.001f;
        if (deccTime == 0) deccTime = 0.001f;
    }

    private bool isMovingToWall(float direction) => isWallLeft && direction < 0 || isWallRight && direction > 0;

    #region Outside Controll

    /// <summary>
    /// Sets the direction where the player moves at the x-Axis. Setting a value lower as -1 or higher as 1 gets clamped to -1/1.
    /// <para>Saves the value that was set and therefore can be called using GetKeyDown and GetKeyUp to save performance. Also GetAxis can be used directly in the brackets</para>
    /// </summary>
    /// <param name="move">The move direction. If its lower when 1/-1, the player moves at lower speed</param>
    public void SetMove(float move) => this.move = Mathf.Clamp(move, -1, 1) * (isCrouching ? crouchSlow : 1);

    /// <summary>
    /// Lets the player start (wall)Jumping
    /// </summary>
    public void Jump() 
    { 
        if (!(isGrounded || isSliding)) return;

        // Depending on if the player is on a wall or floor, start (wall-)jumping
        if (isGrounded) 
        {
            isJumping = true;
            jumpTimer = jumpDurr;   // Start the Countdown
        }
        else 
        {
            isWallJumping = true;
            SetSliding(false);                  // Disables the sliding
            jumpTimer = wallJumpDurr;           // Start the Countdown
            direction = isWallRight ? -1 : 1;   // In which direction the player jumps
        }

    }

    /// <summary>
    /// The jump lasts for a certain amount of seconds, this method cancles the jump early
    /// <para>Note that this method does not cancel wallJumping</para>
    /// </summary>
    public void CancelJump() 
    {
        if(!isJumping) return;

        isJumping = false;
    }

    /// <summary>
    /// Toggles the Crouching on or off.
    /// <para>Its recommented to use it with GetKeyDown and GetKeyUp or something similar</para>
    /// </summary>
    public void ToggleCrouch() => SetCrouching(!isCrouching);

    #endregion
}
