using System.Collections;
using UnityEngine;

public class BattleController : StateMachine {

    public CameraRig cameraRig;
    public Level level;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    public GameObject hero;
    public Unit currentUnit;
    public Tile currentTile { get { return level.GetTile(pos); } }

    private void Start() {
        ChangeState<InitBattleState>();
    }
}
