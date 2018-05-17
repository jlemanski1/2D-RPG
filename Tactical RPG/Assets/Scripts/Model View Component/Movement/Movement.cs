using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Baseclass for unit movement. Limits how many tiles a unit can move, as well as
/// how high the unit can jump up tiles. Will contain other stats in the future
/// </summary>
public abstract class Movement : MonoBehaviour {

    public int range;           // Tile movement range
    public int jumpHeight;      // Height allowed to "jump" up tiles

    protected Unit unit;            // The Unit
    protected Transform jumper;     // The Unit's jumper parent empty object

    // Tells the component to handle the animation of actually traversing a path
    public abstract IEnumerator Traverse(Tile tile);


    /// <summary>
    /// Set up unit and jumper references
    /// </summary>
    protected virtual void Awake() {
        unit = GetComponent<Unit>();
        jumper = transform.Find("Jumper");
    }


    /// <summary>
    /// Determines what tiles are reachable on a given level
    /// </summary>
    /// <param name="level">The current level</param>
    /// <returns></returns>
    public virtual List<Tile> GetTilesInRange(Level level) {
        List<Tile> retValue = level.Search(unit.tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }


    /// <summary>
    /// Compares the distance traveled against the range of the character.
    /// </summary>
    /// <param name="from">Tile to move from</param>
    /// <param name="to">Tile to move to</param>
    /// <returns></returns>
    protected virtual bool ExpandSearch(Tile from, Tile to) {
        return (from.distance + 1) <= range;
    }


    /// <summary>
    /// Loops thorugh the list of tiles returned by a level search, and removes
    /// any which hold blocking content.
    /// </summary>
    /// <param name="tiles">List of tiles return by a level search</param>
    protected virtual void Filter(List<Tile> tiles) {
        for (int i = tiles.Count - 1; i >= 0; i--) {
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
        }
    }


    /// <summary>
    /// Rotates a character to face a given direction by rotating the fastest direction to get there
    /// </summary>
    /// <param name="dir">direction to face</param>
    /// <returns></returns>
    protected virtual IEnumerator Turn(Directions dir) {
        TransformLocalEulerTweener tweener = (TransformLocalEulerTweener)transform.RotateToLocal(
            dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

        // When rotating between N & W, make an exception so it looks like the unit

        // Rotates the most efficient way (0 & 360 are treated the same)
        if (Mathf.Approximately(tweener.startValue.y, 0f) && Mathf.Approximately(tweener.endValue.y, 270f))
            tweener.startValue = new Vector3(tweener.startValue.x, 360f, tweener.startValue.z);
        else if (Mathf.Approximately(tweener.startValue.y, 270) && Mathf.Approximately(tweener.endValue.y, 0))
            tweener.endValue = new Vector3(tweener.startValue.x, 260f, tweener.startValue.z);

        unit.dir = dir;

        while (tweener != null)
            yield return null;
    }
}
