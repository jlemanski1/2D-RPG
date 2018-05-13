using System.Collections;
using UnityEngine;

/// <summary>
/// State that the BattleController will start with. Creates and loads everything that's 
/// necessary, then triggers the next state when its complete
/// </summary>
public class InitBattleState : BattleState {

    public override void Enter() {
        base.Enter();   // Add Listeners
        StartCoroutine(Init());
    }

    IEnumerator Init() {
        level.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        yield return null;
        owner.ChangeState<MoveTargetState>();
    }
}
