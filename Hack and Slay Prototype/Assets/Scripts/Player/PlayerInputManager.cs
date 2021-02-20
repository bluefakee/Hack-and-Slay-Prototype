using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInputManager : MonoBehaviour
{
    private PlayerMovement moveComp;

    private InputMaster master;

    void Awake()
    {
        // Initialize the master
        master = new InputMaster();

        // Get the moveComp
        moveComp = GetComponent<PlayerMovement>();

        // Subscribe to Movement events
        master.Ingame.Movement.started += _ => moveComp.SetMove(_.ReadValue<float>());
        master.Ingame.Movement.canceled += _ => moveComp.SetMove(0);

        // Subscribe to Jump events
        master.Ingame.Jump.started += _ => moveComp.Jump();
        master.Ingame.Jump.canceled += _ => moveComp.CancelJump();

        // Subscribe to Crouch events
        master.Ingame.Crouch.started += _ => moveComp.ToggleCrouch();
        master.Ingame.Crouch.canceled += _ => moveComp.ToggleCrouch();
    }

    // Prevent that master events call methods and cause weird behaviour or exceptions
    void OnEnable()
    {
        master.Enable();
    }

    void OnDisable()
    {
        master.Disable();
    }

    void OnDestroy()
    {
        master.Disable();
    }
}
