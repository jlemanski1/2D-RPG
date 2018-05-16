using System.Collections;
using UnityEngine;

/// <summary>
/// Will hold a reference to all Unit data such as stats, skills, etc. Tracks where the unit
/// is in the level and what direction it's facing
/// </summary>
public class Unit : MonoBehaviour {

	public Tile tile { get; protected set; }
    public Directions dir;

    /// <summary>
    /// Places target GameObject on the tile
    /// </summary>
    /// <param name="target"></param>
    public void Place(Tile target) {
        // Check that old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        // Link unit and tile references
        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    /// <summary>
    /// Match position and angle with the center of the tile
    /// </summary>
    public void Match() {
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }
}
