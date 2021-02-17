using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Referenzes"), SerializeField, Tooltip("Put the movement Component here")]
    private Movement moveComp;

    private InputMaster master;

    void Awake()
    {
        // Initialize the master
        master = new InputMaster();

        // Subscribe Movement to the master
        master.Ingame.Movement.performed += _ => moveComp.SetMove(_.ReadValue<float>());
        master.Ingame.Movement.canceled += _ => moveComp.SetMove(0);            // Reset the move if no movement button is pressed
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
