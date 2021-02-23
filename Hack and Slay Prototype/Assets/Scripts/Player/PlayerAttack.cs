using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField, Tooltip("How large is the hitbox of the attack")]
    private Vector2 hitbox;

    [SerializeField, Range(0f, 3f), Tooltip("The offset in the direction of the attack")]
    private float offset;

    [SerializeField, Range(0f, 1f), Tooltip("How long does the hitbox stay")]
    private float slideDurr;

    private void Update()
    {
        
    }

    #region Outside Controll

    public void Attack(Vector2 direction)
    {

    }

    #endregion
}
