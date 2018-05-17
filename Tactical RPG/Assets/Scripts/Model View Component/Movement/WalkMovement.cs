using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement subclass for units that can walk and are limited by enemies, objects,
/// or obstacles on tiles.
/// </summary>
public class WalkMovement : Movement {

    /// <summary>
    /// Retain the basic distance check of ExpandSearch while making sure that
    /// the unit can jump the height of the tile and that the tile is not occupied
    /// </summary>
    protected override bool ExpandSearch(Tile from, Tile to) {
        // Skip if unit can jump that high
        if ((Mathf.Abs(from.height - to.height) > jumpHeight))
            return false;

        // Skip if tile is occupied by an enemy or misc object
        if (to.content != null)
            return false;

        return base.ExpandSearch(from, to);
    }


    /// <summary>
    /// Animate movement along a path (implemented using nested coroutines)
    /// </summary>
    public override IEnumerator Traverse(Tile tile) {
        unit.Place(tile);

        // Build a list of waypoints from the unit's starting tile to the destination tile
        List<Tile> targets = new List<Tile>();
        while (tile != null) {
            targets.Insert(0, tile);
            tile = tile.prev;
        }

        // Move to each way point in succession
        for (int i = 1; i < targets.Count; i++) {
            Tile from = targets[i - 1];
            Tile to = targets[i];

            Directions dir = from.GetDirection(to);
            if (unit.dir != dir)
                yield return StartCoroutine(Turn(dir));

            if (from.height == to.height)
                yield return StartCoroutine(Walk(to));
            else
                yield return StartCoroutine(Jump(to));
        }
        yield return null;
    }


    /// <summary>
    /// Animate walking movement
    /// </summary>
    /// <param name="target">Tile to walk to</param>
    IEnumerator Walk(Tile target) {
        Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }


    /// <summary>
    /// Animate jumping movement
    /// </summary>
    /// <param name="to">Tile to jump to</param>
    IEnumerator Jump(Tile to) {
        Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);
        Tweener tweener2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0),
            tweener.easingControl.duration / 2f, EasingEquations.EaseOutQuad);
        tweener2.easingControl.loopCount = 1;
        tweener2.easingControl.loopType = EasingControl.LoopType.PingPong;

        while (tweener != null)
            yield return null;
    }
}
