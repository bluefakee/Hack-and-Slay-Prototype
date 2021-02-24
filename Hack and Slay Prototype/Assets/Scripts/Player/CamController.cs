using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float speed;

    private void Update() => transform.position += (Vector3)((Vector2)player.position - (Vector2)transform.position) * speed * Time.deltaTime;
}
