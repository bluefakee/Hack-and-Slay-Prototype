using UnityEngine;

/// <summary>
/// This interface is needed to count as enemy
/// </summary>
public interface IEnemy
{
    /// <summary>
    /// Gets called when the player hits this target
    /// </summary>
    void OnHit();
}
