using UnityEngine;

// this component handles a movement with configureable acceleration and decelearation

public class PlayerMovement : Movement
{
    [Header("Movement"), SerializeField, Tooltip("How fast the maximum velocity of the player is")]
    private float moveSpeed;
    [SerializeField, Tooltip("How long does it take to reach max speed")]
    private float accTime;
    [SerializeField, Tooltip("How long does it take to stop moving when you where at max speed")]
    private float deccTime;

    private float timer;        // a counter for the acc and decc of the object, when it reaches 1 or -1, the max velocity is reached

    void Update()
    {

        #region Movement Calculation

        if (move > 0)
        {
            // move to the right
            if (timer >= 0)
            {
                // Accelerate
                timer = Mathf.Clamp(timer + Time.deltaTime * accTime, -1);
            }
            else
            {
                // The character has moved in the other direction and needs to slow down first
                timer += Time.deltaTime * deccTime;
            }
        }
        else if (move < 0)
        {
            // move to the left
            if (timer >= 0)
            {
                // Accelerate
                timer -= Time.deltaTime * accTime;
            }
            else
            {
                // The character has moved in the other direction and needs to slow down first
                timer -= Time.deltaTime * deccTime;
            }
        }
        else
        {
            // no input -> the character slows down
            if(timer > 0)
            {
                timer = Mathf.Clamp(timer - Time.deltaTime, 0, 1);
            }
            else if (timer < 0)
            {
                timer = Mathf.Clamp(timer + Time.deltaTime, -1, 0);
            }
        }

        #endregion

    }

    void FixedUpdate()
    {
        // Change the velocity. The Sin results in a smooth curve
        rb.velocity = new Vector2(Mathf.Sin(timer * 0.5f * Mathf.PI), rb.velocity.y);
    }
}
