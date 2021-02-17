using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class all movement scripts are inheriting from.
/// <para>This class on its own is useless</para>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    /// <summary>
    /// The referenze to the Rigidbody 
    /// </summary>
    protected Rigidbody2D rb;

    /// <summary>
    /// Saves the value of SetMove
    /// </summary>
    protected float move;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// This sets the direction where the character this script is attached to moves.
    /// <para>Note that the Movement does not get reset and needs to be set to 0</para>
    /// </summary>
    /// <param name="move">what direction is the player moving</param>
    public void SetMove(float move) => Debug.Log(move); // this.move = move;
}
