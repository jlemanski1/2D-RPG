using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State becomes active after the game has decided what unit should take its turn. Highlights
/// the tiles within the unit's range and exits when a valid move is chosen ("fire" input)
/// Unhighlights the tiles before exiting
/// </summary>
public class MoveTargetState : BattleState {

    List<Tile> tiles;

    public override void Enter() {
        base.Enter();
        Movement mover = owner.currentUnit.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(level);
        level.SelectTiles(tiles);
    }


    public override void Exit() {
        base.Exit();
        level.DeSelectTiles(tiles);
        tiles = null;
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e) {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e) {
        if (tiles.Contains(owner.currentTile))
            owner.ChangeState<MoveSequenceState>();
    }
}
