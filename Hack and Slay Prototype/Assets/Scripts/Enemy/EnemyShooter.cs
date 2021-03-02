using UnityEngine;

public class EnemyShooter : MonoBehaviour, IEnemy
{
    public void OnHit()
    {
        Debug.Log("Enemy killed");
        Destroy(gameObject);
    }


}
