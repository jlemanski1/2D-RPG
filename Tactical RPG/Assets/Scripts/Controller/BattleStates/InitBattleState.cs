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
        SpawnTestUnits();
        yield return null;
        owner.ChangeState<SelectUnitState>();
    }

    
    /// <summary>
    /// Test of Unit Spawning
    /// </summary>
    void SpawnTestUnits() {
        System.Type[] components = new System.Type[] { typeof(WalkMovement), typeof(FlyMovement),
        typeof(TeleportMovement)};
        // Instantiate 3 copies of the hero prefab
        for (int i = 0; i < 3; i++) {
            GameObject instance = Instantiate(owner.hero) as GameObject;

            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(level.GetTile(p));
            unit.Match();

            Movement move = instance.AddComponent(components[i]) as Movement;
            move.range = 5;
            move.jumpHeight = 1;
        }
    }
}
