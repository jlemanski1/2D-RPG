using System.Collections;
using UnityEngine;

/// <summary>
/// Won't be part of the final game, just for testing purposes. For now select any demo units
/// by moving the cursor onto them and selecting them using the "fire" input
/// </summary>
public class SelectUnitState : BattleState {

    protected override void OnMove(object sender, InfoEventArgs<Point> e) {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e) {
        GameObject content = owner.currentTile.content;
        if (content != null) {
            owner.currentUnit = content.GetComponent<Unit>();
            owner.ChangeState<MoveTargetState>();
        }
    }
}
