using UnityEngine;

[RequireComponent(typeof(EnemyAggro), typeof(BoxCollider2D))]
public class EnemyShooter : MonoBehaviour, IEnemy
{
    // Referenzes 
    private EnemyAggro aggro;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField, Tooltip("How often does the player shoot")]
    private float secondsPerShoot = 1;

    private float counter;

    public void OnHit()
    {
        Debug.Log("Enemy killed");
        Destroy(gameObject);
    }

    private void Update()
    {
        if (aggro.isInSight && counter == 0)    // The player is in sight and the shootcooldown has run down
        {
            // Shoot to the player direction
            Quaternion rot = Quaternion.Euler(0, 0, Mathf.Acos(aggro.dirToPlayer.y / Mathf.Sqrt(Mathf.Pow(aggro.dirToPlayer.x, 2) + Mathf.Pow(aggro.dirToPlayer.y, 2))) * Mathf.Rad2Deg * (aggro.dirToPlayer.x > 0 ? -1 : 1));

            Instantiate(bulletPrefab, transform.position, rot);
            counter = secondsPerShoot;
        }
        
        if (counter != 0)                  // The shoot cooldown has not run down
        {
            // Count down
            counter = Mathf.Clamp(counter - Time.deltaTime, 0, secondsPerShoot);
        }
    }

    public void Awake()
    {
        aggro = GetComponent<EnemyAggro>();
    }
}
