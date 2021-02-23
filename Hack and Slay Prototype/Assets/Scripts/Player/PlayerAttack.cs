﻿using UnityEngine;
using System.Collections;           // IEnumerator interface

// This script must be attached to the attackCollider

public class PlayerAttack : MonoBehaviour
{
    [SerializeField, Tooltip("How long does the slice last")]
    private float durr;

    private BoxCollider2D coll;

    private bool isAttacking { get => coll.enabled; set => coll.enabled = value; }

    private IEnumerator _Attack(Vector2 direction)
    {
        // Bypass the attack if its already getting executed
        if (isAttacking) yield break;

        // Apply the direction as rotation to the transform to move the collider (note that i needed to multiply the angle by -1 for reasons)
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Acos(direction.y / Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))) * Mathf.Rad2Deg * (direction.x > 0 ? -1 : 1));

        // Activate the collider to register collision
        coll.enabled = true;

        // Change the layer to attack layer so it can register enemys (see projectsettings.Physics2D)
        coll.gameObject.layer--;

        yield return new WaitForSeconds(durr);

        // Reverse changes made
        coll.enabled = false;
        coll.gameObject.layer++;

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Object getroffen: " + coll.name);
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
