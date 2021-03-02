using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    [SerializeField, Range(0f, 100f), Tooltip("How far can the enemy see")]
    private float sightRange;

    [SerializeField, Tooltip("What can the enemy not see through")]
    LayerMask whatBlocksSight;

    /// <summary>
    /// Has the enemy aggro on player
    /// </summary>
    public bool isAggro { get; private set; }

    /// <summary>
    /// Is the player in sight?
    /// </summary>
    public bool isInSight { get; private set; }

    /// <summary>
    /// Returns the normalized direction to the player
    /// </summary>
    public Vector2 dirToPlayer { get => dir.normalized; }

    private Vector2 dir;

    private void Update()
    {
        dir = PlayerInputManager.playertrans.position - transform.position;
        RaycastHit2D result = Physics2D.Raycast(transform.position, dir, sightRange, whatBlocksSight);

        if (result.collider?.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isAggro = true;
            isInSight = true;
        }
        else
        {
            isInSight = false;
        }

        Debug.DrawRay(transform.position, dir);
    }

    public void ResetAggro() => isAggro = false;
}
