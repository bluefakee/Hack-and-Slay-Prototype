using UnityEngine;
using UnityEngine.InputSystem;          // Getting the mousePosition for dashing

[RequireComponent(typeof(PlayerMovement), typeof(PlayerSlowmoManager), typeof(PlayerDashManager))]
public class PlayerInputManager : MonoBehaviour
{
    public static Transform playertrans { get; private set; }

    private InputMaster master;

    private PlayerMovement moveComp;
    private PlayerSlowmoManager slowmoComp;
    private PlayerDashManager dashComp;

    [Header("Referenzes"), SerializeField]
    private PlayerAttack attackComp;

    [SerializeField]
    private Transform playerCenter;

    private void Awake()
    {
        playertrans = playerCenter == null ? transform : playerCenter;

        // Get the referenzes
        moveComp = GetComponent<PlayerMovement>();
        slowmoComp = GetComponent<PlayerSlowmoManager>();
        dashComp = GetComponent<PlayerDashManager>();

        // Initialize the master
        master = new InputMaster();

        // Subscribe to Movement events
        master.Ingame.Movement.performed += _ => moveComp.SetMove(_.ReadValue<float>());
        master.Ingame.Movement.canceled += _ => moveComp.SetMove(0);

        // Subscribe to Jump events
        master.Ingame.Jump.started += _ => moveComp.Jump();
        master.Ingame.Jump.canceled += _ => moveComp.CancelJump();

        // Subscribe to Crouch events
        master.Ingame.Crouch.started += _ => moveComp.Crouch(true);
        master.Ingame.Crouch.canceled += _ => moveComp.Crouch(false);

        // Subscribe to RunUp events
        master.Ingame.RunUp.started += _ => moveComp.RunUp(true);
        master.Ingame.RunUp.canceled += _ => moveComp.RunUp(false);

        // Subscribe to Dash events
        master.Ingame.Dash.started += _ => dashComp.Dash(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position);

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
