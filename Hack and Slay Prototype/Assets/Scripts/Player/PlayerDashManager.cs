using System;
using UnityEngine;

// This script handels the Dashcooldown and changes the dashdirection (when it is sligthly above when grounded, it gets set to 1,0/-1,0)

public class PlayerDashManager : MonoBehaviour
{
    /// <summary>
    /// Gets called when canDash gets updated and passes canDash
    /// </summary>
    public static Action<bool> UpdateDash;

    /// <summary>
    /// Gets called when the internal counter gets updated. The value passed in is the counter / recover (a value between 0 and 1)
    /// </summary>
    // No pog for you!
    public static Action<float> RecoverProg;

    [SerializeField, Range(0f, 1f), Tooltip("When does the player regenerate his dash")]
    private float recover;

    [SerializeField, Range(0f, 1f), Tooltip("How much can this script override the dashdirection if needed")]
    private float dashCorrect;

    private bool canDash = true;
    private float counter;
    private PlayerMovement comp;

    private void Awake() => comp = GetComponent<PlayerMovement>();

    private void Update()
    {
        if (!canDash)
        {
            // Not max dashes
            if (comp.isTouchingGround)  // Is touching ground
            {
                // Count down
                counter -= Time.deltaTime;

                if (counter <= 0)       // Counted down
                {
                    // Reenable dash
                    canDash = true;
                    UpdateDash?.Invoke(canDash);
                }

                RecoverProg?.Invoke(Mathf.Clamp01(counter / recover));
            }
            else                        // Isnt touching ground
            {
                // Reset the counter
                counter = recover;
                RecoverProg?.Invoke(1);
            }
        }
    }

    public void Dash(Vector2 direction)
    {
        // Bypass if not ready to dash
        if (!canDash) return;

        direction.Normalize();

        // Modify direction if needed
        if (comp.isGrounded && direction.y < dashCorrect && direction.y > 0 || comp.isCeilingAbove && direction.y > -dashCorrect && direction.y < 0)     // If the direction is a little bit away from a Ceiling or Ground (how much is set by dashCorrect)
        {
            direction.y = 0;
        }
        if (comp.isWallLeft && direction.x < dashCorrect && direction.x > 0 || comp.isWallRight && direction.x > -dashCorrect && direction.x < 0)        // Same like above for walls
        {
            direction.x = 0;
        }

        // Execute dash
        comp.Dash(direction);
        counter = recover;
        canDash = false;
        UpdateDash?.Invoke(canDash);
    }
}
