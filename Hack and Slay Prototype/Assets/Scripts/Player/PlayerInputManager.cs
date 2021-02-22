using UnityEngine;
using UnityEngine.InputSystem;          // Getting the mousePosition for dashing

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInputManager : MonoBehaviour
{
    #region Events



    #endregion

    private PlayerMovement moveComp;

    private InputMaster master;

    #region Dash Fields

    [Header("Dashmanagement"), SerializeField, Range(0f, 3f), Tooltip("How long must the player be grounded until he starts to recover his dashUses")]
    private float dashRecoverStart = 0.35f;

    [SerializeField, Range(0f, 1f), Tooltip("How long does it take until a dashUse is recovered")]
    private float dashRecoverTime = 0.1f;

    private int dashUses;
    private float counter;

    private void ResetCounter() => counter = dashRecoverStart + dashRecoverTime;

    #endregion

    #region Debug

    [Header("Debug")]
    public bool debugStats;

    private void OnGUI()
    {
        if (debugStats)
        {
            int y = 0;
            GUI.Box(new Rect(0, y, 200, 25), "Dashes: " + dashUses.ToString()); y += 25;
            GUI.Box(new Rect(0, y, 200, 25), "Counter: " + counter.ToString()); y += 25;
        }
    }

    #endregion

    private void Update()
    {
        #region Dash

        if (dashUses != 3)
        {
            if (moveComp.isGrounded)
            {
                // Recover dashes
                counter -= Time.deltaTime;

                if (counter <= 0)
                {
                    // Counted down => recover a dashUse
                    dashUses++;
                    counter = dashRecoverTime;
                }
            }
            else
            {
                // Reset the counter
                ResetCounter();
            }
        }

        #endregion
    }

    private void Awake()
    {
        ResetCounter();

        // Initialize the master
        master = new InputMaster();

        // Get the referenzes
        moveComp = GetComponent<PlayerMovement>();

        // Subscribe to Movement events
        master.Ingame.Movement.started += _ => moveComp.SetMove(_.ReadValue<float>());
        master.Ingame.Movement.canceled += _ => moveComp.SetMove(0);

        // Subscribe to Jump events
        master.Ingame.Jump.started += _ => moveComp.Jump();
        master.Ingame.Jump.canceled += _ => moveComp.CancelJump();

        // Subscribe to Crouch events
        master.Ingame.Crouch.started += _ => moveComp.Crouch(true);
        master.Ingame.Crouch.canceled += _ => moveComp.Crouch(false);

        // Subscribe to Dash events
        master.Ingame.Dash.started += _ =>
        {
            if (dashUses == 0) return;
            moveComp.Dash(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position);
            dashUses--;
            ResetCounter();
        };
    }

    // Prevent that master events call methods and cause weird behaviour or exceptions
    private void OnEnable()
    {
        master.Enable();
    }

    private void OnDisable()
    {
        master.Disable();
    }

    private void OnDestroy()
    {
        master.Disable();
    }
}
