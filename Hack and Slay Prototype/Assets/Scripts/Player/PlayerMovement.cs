﻿using System.Collections;       // IEnumerator interface
using UnityEngine;

// PlayerMovement writen by bluefake
// In Update all regions have a explanation of what the code there does
// If you experience bugs and things that you think shouldnt happen or just need help, just dm me on discord (bluefake#3507)

// Work In Progress:
// Eventsystem for Animations
// Crouch does not change the position of the checks

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    #region Events

    // Insert funny code here

    #endregion

    #region Groundcheck Fields

    [Header("Groundchecks"), SerializeField]
    private Transform feet;

    [SerializeField]
    private Transform head, left, right;

    [SerializeField, Tooltip("Its recommended to make the as wide as possible to make the checks more responsible. But using the max Length can result in weird behaviour because the player is for example grounded at a wall")]
    private Vector2 feetSize, headSize, leftSize, rightSize;

    [SerializeField]
    private LayerMask whatIsGround;

    public bool isGrounded { get; private set; }            // Is the player on Ground?
    public bool isCeilingAbove { get; private set; }        // Is the player hitting a Ceiling?
    public bool isWallLeft { get; private set; }            // Is the player touching a wall left
    public bool isWallRight { get; private set; }           // Is the player touching a wall right?
    private bool groundCallChecker;                         // This is a field needed for the OnStartGround and OnEndGround methods

    /// <summary>
    /// Returns true if any check is true
    /// </summary>
    public bool isTouchingGround => isGrounded || isCeilingAbove || isWallLeft || isWallRight;

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

    [SerializeField, Range(0f, -20f), Tooltip("How fast can the player fall")]
    private float maxDrag;

    [SerializeField, Range(0f, 1f), Tooltip("A multiplyer that gets applied to the acc and decc when the player is not grounded")]
    private float midAirSlow = 0.5f;

    private float jumpTimer;            // Counts the time down while the object jumps (both walljump and normal)

    private bool isJumping;             // has the Player started Jump()?

    #endregion

    #region WallSlide Fields

    [Header("Wallsliding"), SerializeField, Range(0f, 1f), Tooltip("The time the player needs to hold the other direction in order to get away from a wall")]
    private float unAttachDurr;

    [SerializeField, Range(0f, 1f), Tooltip("With what gets the grav of the player multiplyed when wallSliding")]
    private float slideGravMulti = 0.5f;

    [SerializeField, Range(0f, -20f), Tooltip("How high can be the negative velocity.y when sliding")]
    private float maxSlideDrag;

    [SerializeField, Range(0f, 100f), Tooltip("How fast is the fallspeed reduced per second if it the player falls faster when the maxSpeed")]
    private float fallRecover;

    private float unAttachTimer;    // Countdown until the sliding gets canceled
    private int direction;          // Saves where the player is jumping to

    private bool isSliding;         // Its recommended to use SetSlide since it also changes the gravity

    private void SetSlide(bool value)
    {
        if (isSliding == value) return;

        isSliding = value;

        if (!isCrouching) rb.gravityScale *= isSliding ? slideGravMulti : (1 / slideGravMulti);
    }

    #endregion

    #region WallJump Fields

    [Header("Walljumping"), SerializeField, Range(0f, 10f), Tooltip("How fast is the velocity when jumping of a wall")]
    private float wallJumpForce;

    [SerializeField, Range(0f, 1f), Tooltip("How long does the wallJump get executed (note that it cannot be canceled)")]
    private float wallJumpDurr;

    private bool isWallJumping;     // gets true instead of isJumping when Jump() called and on a wall

    #endregion

    #region Crouch Fields

    [Header("Crouch"), SerializeField, Range(0f, 1f), Tooltip("with what gets the collider size multiplyed with when crouching")]
    private float ySizeMulti;

    [SerializeField, Range(0f, 1f), Tooltip("With what gets the grav multiplyed when crouching on floor to better slide")]
    private float crouchGravMulti;

    private bool isCrouching;       // Its recommended to use this similar to isSliding

    private void SetCrouch(bool value)
    {
        if (value == isCrouching) return;

        isCrouching = value;

        // Allow only to change the value when dashing
        if (isDashing) return;

        // Assigns new Collider size
        coll.size = new Vector2(coll.size.x, coll.size.y * (isCrouching ? ySizeMulti : 1 / ySizeMulti));
        coll.offset += new Vector2(0, isCrouching ? -coll.size.y * 0.5f : coll.size.y * 0.5f * ySizeMulti);

        // This recalculates the timer to fit the rb.velocity after crouching
        if (!isCrouching) timer = VelocityToTimer(rb.velocity.x);

        // Toggle gravity when sliding
        if (isSliding) rb.gravityScale *= isCrouching ? (1 / slideGravMulti) : slideGravMulti;

        if (isGrounded) rb.gravityScale *= isCrouching ? crouchGravMulti : (1 / crouchGravMulti);
    }

    #endregion

    #region Dash Fields

    [Header("Dash"), SerializeField, Range(0f, 10f), Tooltip("How far can the player dash")]
    private float dashRange;

    [SerializeField, Range(0f, 1f), Tooltip("For how long can the player dash")]
    private float dashDurr;

    [SerializeField, Tooltip("The boost the player gets normally after a dash")]
    private Vector2 dashBoost;

    [SerializeField, Range(0f, 1f), Tooltip("With what gets the velocity multiplyed if the momentum of the dash is kept")]
    private float dashMomentum;

    private bool isDashing;

    private float defaultGrav;

    #endregion

    #region Debugging

    [Header("Debugging"), SerializeField, Tooltip("Shows the hitboxes of the checks in the Scene")]
    private bool debugChecks;

    [Tooltip("If enabled, a GUI will be generated on the top left that contains stats like the timer, if the player isGrounded etc")]
    public bool debugStats;

    private void OnDrawGizmos()
    {
        if (debugChecks)
        {
            Gizmos.DrawCube(feet.position, new Vector3(feetSize.x, feetSize.y, 0.1f));
            Gizmos.DrawCube(head.position, new Vector3(headSize.x, headSize.y, 0.1f));
            Gizmos.DrawCube(left.position, new Vector3(leftSize.x, leftSize.y, 0.1f));
            Gizmos.DrawCube(right.position, new Vector3(rightSize.x, rightSize.y, 0.1f));
        }
    }

    private void OnGUI()
    {
        if (debugStats)
        {
            GUILayout.Box("Velocity = " + rb.velocity.ToString());
            GUILayout.Box("Gravity Scale = " + rb.gravityScale.ToString());
            GUILayout.Box("Input Move = " + move.ToString());
            GUILayout.Box("Internal Move = " + timer.ToString());
            GUILayout.Box("isGrounded = " + isGrounded.ToString());
            GUILayout.Box("isCeilingAbove = " + isCeilingAbove.ToString());
            GUILayout.Box("isWallLeft = " + isWallLeft.ToString());
            GUILayout.Box("isWallRight = " + isWallRight.ToString());
            GUILayout.Box("isSliding = " + isSliding.ToString());
            GUILayout.Box("isJumping = " + isJumping.ToString());
            GUILayout.Box("isWallJumping = " + isWallJumping.ToString());
            GUILayout.Box("isCrouching = " + isCrouching.ToString());
            GUILayout.Box("isDashing = " + isDashing.ToString());
        }
    }

    #endregion

    private void Update()
    {
        #region Groundcheck

        // The groundcheck checks in a box of the size that you can set in the inspector if there is any ground
        isGrounded = Physics2D.OverlapBox(feet.position, feetSize, 0, whatIsGround);
        isCeilingAbove = Physics2D.OverlapBox(head.position, headSize, 0, whatIsGround);
        isWallLeft = Physics2D.OverlapBox(left.position, leftSize, 0, whatIsGround);
        isWallRight = Physics2D.OverlapBox(right.position, rightSize, 0, whatIsGround);

        if (isGrounded && !groundCallChecker)
        {
            // Gets called once when becoming grounded
            groundCallChecker = true;

            // Lower grav when crouching
            if (isCrouching) rb.gravityScale *= crouchGravMulti;
        }

        if (!isGrounded && groundCallChecker)
        {
            // Gets called once when becoming ungrounded
            groundCallChecker = false;
        }

        #endregion

        #region Timer Calculation

        if (move > 0 && timer <= move && (!isCrouching || !isGrounded))
        {
            // the timer is not bigger as move and the player inputs right, timer increases intil it reaches move
            timer = Mathf.Clamp(timer + Time.deltaTime * (isGrounded ? 1 : midAirSlow) / accTime, -1, move);
        }
        else if (move < 0 && timer >= move && (!isCrouching || !isGrounded))
        {
            // same like above, only for left and decreasing
            timer = Mathf.Clamp(timer - Time.deltaTime * (isGrounded ? 1 : midAirSlow) / accTime, move, 1);
        }
        else if (isGrounded)
        {
            // no input or the timer results in faster movement when the inputs allow -> the timer nears to 0
            timer = timer > 0 ? Mathf.Clamp(timer - Time.deltaTime * (isGrounded ? 1 : midAirSlow) / deccTime, 0, 1) :
                                Mathf.Clamp(timer + Time.deltaTime * (isGrounded ? 1 : midAirSlow) / deccTime, -1, 0);
        }

        // Prevent that the player gets stuck on walls
        if (isMovingToWall(timer)) timer = 0;

        #endregion

        #region Jump Management

        if ((isJumping || isWallJumping) && jumpTimer > 0 && !isCeilingAbove && !isDashing)
        {
            // Count down the time the player is jumping up
            jumpTimer -= Time.deltaTime;
        }
        else if (isJumping && !isDashing)
        {
            // Cancel jump
            isJumping = false;
        }
        else if (isWallJumping && !isDashing)
        {
            // Cancel wallJump
            isWallJumping = false;
            timer = direction;
        }

        #endregion

        #region WallSliding

        if (!isGrounded && !isSliding && !isDashing && ((isMovingToWall(move) && !isWallJumping) || (isMovingToWall(direction) && isWallJumping)))
        {
            // The player is moving against a wall while not grounded or wallJumps at a wall and starts sliding
            SetSlide(true);

            // Prevents weird hehaviour
            if (isWallJumping) isWallJumping = false;
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

        if ((isGrounded || unAttachTimer <= 0 || !isWallRight && !isWallLeft) && isSliding)
        {
            // Reset the slide
            SetSlide(false);
        }

        #endregion
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        // Get the velocity
        Vector2 velocity = rb.velocity;

        if (velocity.y < (isSliding && !isCrouching ? maxSlideDrag : maxDrag))
        {
            // Limit the fallspeed
            velocity.y += fallRecover * Time.fixedDeltaTime;
        }

        if ((!isCrouching || isJumping) && !isWallJumping)
        {
            velocity.x =  rb.velocity.x < moveSpeed && rb.velocity.x > -moveSpeed ? TimerToVelocity(timer) : velocity.x - velocity.x * deccTime * Time.fixedDeltaTime;
        }

        if (isJumping)
        {
            // Apply jumpForce to velocity.y to simulate jumping
            velocity.y = jumpForce;
        }

        if (isWallJumping)
        {
            // Apply wallJumpForce to velocity.y
            velocity = new Vector2(direction * moveSpeed, wallJumpForce > rb.velocity.y ? wallJumpForce : rb.velocity.y);
        }

        rb.velocity = velocity;
    }

    IEnumerator _Dash(Vector2 direction)
    {
        // Reset slide since it will else not cancel properly
        SetSlide(false);

        // Save the current state of isCrouching
        bool crouch = isCrouching;

        // Start the dashloop
        isDashing = true;
        rb.gravityScale = 0;
        for (float timePassed = dashDurr; timePassed > 0; timePassed -= Time.fixedDeltaTime)
        {
            rb.velocity = new Vector2(isWallLeft && direction.x < 0 || isWallRight && direction.x > 0 ? 0 : direction.x,
                isGrounded && direction.y < 0 || isCeilingAbove && direction.y > 0 ? 0 : direction.y) * dashRange / dashDurr;

            yield return new WaitForFixedUpdate();
        }
        isDashing = false;

        // Reset gravity
        rb.gravityScale = defaultGrav;

        // Set Sliding if dashed to a wall
        if (isWallLeft || isWallRight)
        {
            SetSlide(true);
        }

        // Change gravity depending on crouch
        rb.gravityScale *= isCrouching && !isSliding ? crouchGravMulti : 1;

        // Change the hitbox size depending on crouch
        if (crouch != isCrouching)
        {
            coll.size = new Vector2(coll.size.x, coll.size.y * (isCrouching ? ySizeMulti : 1 / ySizeMulti));
            coll.offset += new Vector2(0, isCrouching ? -coll.size.y * 0.5f : coll.size.y * 0.5f * ySizeMulti);
        }

        // Sets new velocity
        if (!(isGrounded || isSliding)) rb.velocity = rb.velocity.normalized * dashBoost;
        else rb.velocity *= dashMomentum;

        // Jump got called while dashing -> the jump gets executed afterward
        if (isJumping)
        {
            isJumping = false;
            Jump();
        }

        // Set timer
        timer = VelocityToTimer(rb.velocity.x);
        yield break;
    }

    private void Awake()
    {
        // Get referenzes
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        // Assign default grav
        defaultGrav = rb.gravityScale;

        // Prevent divisions with 0
        if (accTime == 0) accTime = 0.001f;
        if (deccTime == 0) deccTime = 0.001f;
    }

    private bool isMovingToWall(float direction) => isWallLeft && direction < 0 || isWallRight && direction > 0;

    private float VelocityToTimer(float velocity) => Mathf.Asin(Mathf.Clamp(velocity / moveSpeed, -1, 1)) * 2 / Mathf.PI;

    private float TimerToVelocity(float timer) => Mathf.Sin(timer * 0.5f * Mathf.PI) * moveSpeed;

    #region Outside Controll

    /// <summary>
    /// Sets the direction where the player moves at the x-Axis. Setting a value lower as -1 or higher as 1 gets clamped to -1/1.
    /// <para>Saves the value that was set and therefore can be called using GetKeyDown and GetKeyUp to save performance. Also GetAxis can be used directly in the brackets</para>
    /// </summary>
    /// <param name="move">The move direction. If its lower when 1/-1, the player moves at lower speed</param>
    public void SetMove(float move) => this.move = Mathf.Clamp(Mathf.Asin(move) * 2 / Mathf.PI, -1, 1);   // The formula used there is the reversed used in FixedUpdate to make the move the actuall movementspeed

    /// <summary>
    /// Lets the player start (wall)Jumping
    /// </summary>
    public void Jump()
    {
        if (!(isGrounded || isSliding || isDashing)) return;

        // Depending on if the player is on a wall or floor, start (wall-)jumping
        if (isGrounded)
        {
            // This prevents that the low grav from crouching stays
            if (isCrouching) rb.gravityScale *= 1 / crouchGravMulti;

            isJumping = true;
            jumpTimer = jumpDurr;   // Start the Countdown
        }
        else if (isSliding)
        {
            isWallJumping = true;
            SetSlide(false);                    // Disables the sliding
            jumpTimer = wallJumpDurr;           // Start the Countdown
            direction = isWallRight ? -1 : 1;   // In which direction the player jumps
        }
        else
        {
            // Jump got called while dashing and this method gets called again after dashing or even canceles the dash if the player risks to not slide/be grounded anymore
            isJumping = true;
        }

    }

    /// <summary>
    /// The jump lasts for a certain amount of seconds, this method cancles the jump early
    /// <para>Note that this method does not cancel wallJumping</para>
    /// </summary>
    public void CancelJump()
    {
        if (!isJumping) return;

        isJumping = false;
    }

    /// <summary>
    /// Toggles the Crouching on or off.
    /// <para>Its recommented to use it with GetKeyDown and GetKeyUp or something similar</para>
    /// </summary>
    public void Crouch(bool crouch) => SetCrouch(crouch);

    /// <summary>
    /// Lets the player dash in direction. The direction is internally normalized
    /// </summary>
    /// <param name="direction">The dash direction</param>
    public void Dash(Vector2 direction) => StartCoroutine(_Dash(direction.normalized));

    #endregion
}
