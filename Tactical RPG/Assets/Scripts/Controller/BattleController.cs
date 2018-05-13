using System.Collections;
using UnityEngine;

public class BattleController : StateMachine {

    public CameraRig cameraRig;
    public Level level;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    private void Start() {
        ChangeState<InitBattleState>();
    }
}
