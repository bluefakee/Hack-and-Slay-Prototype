using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask whatIsColl;

    private const float bulletSpeed = 10f;

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * bulletSpeed;
    }

    public void ReverseDir()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rot.x, rot.y, -rot.z);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player hit");
        }
        else if (coll.gameObject.layer == whatIsColl)
        {
            // Bullet impact particles
            Debug.Log("Kill");
            Destroy(gameObject);
        }
    }
}
