using UnityEngine;
using UnityEngine.InputSystem;          // Getting the mousePosition for dashing

[RequireComponent(typeof(PlayerMovement), typeof(PlayerSlowmoManager), typeof(PlayerDashManager))]
public class PlayerInputManager : MonoBehaviour
{
    private InputMaster master;

    private PlayerMovement moveComp;
    private PlayerSlowmoManager slowmoComp;
    private PlayerDashManager dashComp;

    [Header("Referenzes"), SerializeField]
    private PlayerAttack attackComp;

    private Vector2 input;

    private void Awake()
    {
        // Get the referenzes
        moveComp = GetComponent<PlayerMovement>();
        slowmoComp = GetComponent<PlayerSlowmoManager>();
        dashComp = GetComponent<PlayerDashManager>();

        // Initialize the master
        master = new InputMaster();
        
        // Subscribe to Movement events
        master.Ingame.Movement.performed += _ =>
        {
            // Save the input
            input = _.ReadValue<Vector2>();

            moveComp.SetMove(input.x);
        };
        master.Ingame.Movement.canceled += _ => moveComp.SetMove(0);

        // Subscribe to Jump events
        master.Ingame.Jump.started += _ => moveComp.Jump();
        master.Ingame.Jump.canceled += _ => moveComp.CancelJump();

        // Subscribe to Crouch events
        master.Ingame.Crouch.started += _ => moveComp.Crouch(true);
        master.Ingame.Crouch.canceled += _ => moveComp.Crouch(false);

        // Subscribe to Dash events
        master.Ingame.Dash.started += _ => dashComp.Dash(input);        // Dash in input direction

        // Subscribe to Slowmo events
        master.Ingame.ToggleSlowmo.started += _ => slowmoComp.ToggleSlowmo();

        // Subscribe to Attack events
        master.Ingame.Attack.started += _ => attackComp.Attack(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position);
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
