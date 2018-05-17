using System.Collections;
using UnityEngine;

/// <summary>
/// Movement class for units that fly, and are not limited by objects or enemies
/// blocking the path
/// </summary>
public class FlyMovement : Movement {

    /// <summary>
    /// Override of the Traverse Animation for flying units. Takes off, flys to tile, then lands.
    /// </summary>
    public override IEnumerator Traverse(Tile tile) {
        // Distance between that start tile and target tile
        float dist = Mathf.Sqrt(Mathf.Pow(tile.pos.x - unit.tile.pos.x, 2) +
            Mathf.Pow(tile.pos.y - unit.tile.pos.y, 2));
        unit.Place(tile);

        // Fly high enough not to clip through any ground tiles
        float y = Tile.stepHeight * 10;
        float duration = (y - jumper.position.y) * 0.5f;
        Tweener tweener = jumper.MoveToLocal(new Vector3(0, y, 0), duration,
            EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;

        // Turn to face the general target direction
        Directions dir;
        Vector3 toTile = (tile.center - transform.position);
        if (Mathf.Abs(toTile.x) > Mathf.Abs(toTile.z))
            dir = toTile.x > 0 ? Directions.East : Directions.West;
        else
            dir = toTile.z > 0 ? Directions.North : Directions.South;
        yield return StartCoroutine(Turn(dir));

        // Move to the correct position
        duration = dist * 0.5f;
        tweener = transform.MoveTo(tile.center, duration, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;

        // Land back on the ground
        duration = (y - tile.center.y) * 0.5f;
        tweener = jumper.MoveToLocal(Vector3.zero, 0.5f, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
    }

}
