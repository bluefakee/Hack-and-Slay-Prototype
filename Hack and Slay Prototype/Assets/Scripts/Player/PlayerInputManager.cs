using System;                           // Actions
using UnityEngine;
using UnityEngine.InputSystem;          // Getting the mousePosition for dashing

[RequireComponent(typeof(PlayerMovement), typeof(PlayerSlowmoManager))]
public class PlayerInputManager : MonoBehaviour
{
    #region Events

    /// <summary>
    /// Passes the number of dashes
    /// </summary>
    public static Action<int> dashCounter;

    /// <summary>
    /// Passes the progress of the dash recovery in percentage from 0 to 1
    /// </summary>
    public static Action<float> dashRecoverProgress;

    /// <summary>
    /// Passes the progress of the initial time that needs to pass in order to recover dashes in percentage from 0 to 1
    /// </summary>
    public static Action<float> dashRecoverStartProgress;

    #endregion

    private InputMaster master;

    #region Referenzes

    private PlayerMovement moveComp;
    private PlayerSlowmoManager slowmoComp;

    [Header("Referenzes"), SerializeField]
    private PlayerAttack attackComp;

    #endregion

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
            GUILayout.Box("Dashes: " + dashUses.ToString());
            GUILayout.Box("Counter: " + counter.ToString());
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

                // Call actions
                if (counter >= dashRecoverStart)
                {
                    dashRecoverStartProgress?.Invoke((counter - dashRecoverTime) / dashRecoverStart);
                    dashRecoverProgress?.Invoke(0);
                }
                else
                {
                    dashRecoverProgress?.Invoke(counter / dashRecoverTime);
                }

                if (counter <= 0)
                {
                    // Counted down => recover a dashUse and call the dashCounter
                    dashUses++;
                    dashCounter?.Invoke(dashUses);
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
        dashUses = 3;

        // Get the referenzes
        moveComp = GetComponent<PlayerMovement>();
        slowmoComp = GetComponent<PlayerSlowmoManager>();

        #region InputMaster

        // Initialize the master
        master = new InputMaster();

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

        // Subscribe to Slowmo events
        master.Ingame.ToggleSlowmo.started += _ => slowmoComp.ToggleSlowmo();

        // Subscribe to Attack events
        master.Ingame.Attack.started += _ => attackComp.Attack(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position);

        #endregion
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
