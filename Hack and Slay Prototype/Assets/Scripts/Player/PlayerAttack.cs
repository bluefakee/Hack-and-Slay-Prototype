using UnityEngine;
using System.Collections;           // IEnumerator interface
using System;

// This script must be attached to the attackCollider

public class PlayerAttack : MonoBehaviour
{
    [SerializeField, Tooltip("How long does the slice last")]
    private float durr;

    private BoxCollider2D coll;

    private bool isAttacking
    {
        get => coll.enabled;
        set
        {
            coll.enabled = value;
            coll.gameObject.layer += value ? -1 : 1;
            transform.GetChild(0).gameObject.SetActive(value);
        }
    }

    private IEnumerator _Attack(Vector2 direction)
    {
        // Bypass the attack if its already getting executed
        if (isAttacking) yield break;

        // Apply the direction as rotation to the transform to move the collider (note that i needed to multiply the angle by -1 for reasons)
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Acos(direction.y / Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))) * Mathf.Rad2Deg * (direction.x > 0 ? -1 : 1));

        // Activate the collider to register collision
        isAttacking = true;

        yield return new WaitForSeconds(durr);

        // Reverse changes made
        isAttacking = false;

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Play animation that the players attack gets interrupted
        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Enemy hit, the try catch prevents exceptions if I was dumb and forgot to inherit IEnemy
            try { coll.GetComponent<IEnemy>().OnHit(); }
            catch (NullReferenceException) { Debug.LogWarning("Warning from PlayerAttack. A object with Enemy layer has no script that inherits from IEnemy"); }
        }
    }

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    #region Outside Controll

    /// <summary>
    /// Starts the attack in direction
    /// </summary>
    /// <param name="direction">The direction of the attack. Gets internally normalized</param>
    public void Attack(Vector2 direction) => StartCoroutine(_Attack(direction));
    #endregion
}
