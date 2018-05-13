using System.Collections;
using UnityEngine;

/// <summary>
/// Base Class for all the states used by BattleController
/// </summary>
public abstract class BattleState : State {

    protected BattleController owner;
    public CameraRig cameraRig { get { return owner.cameraRig; } }
	public Level level { get { return owner.level; } }
    public LevelData levelData { get { return owner.levelData; } }
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }


    protected virtual void Awake() {
        owner = GetComponent<BattleController>();
    }

    protected override void AddListeners() {
        InputController.moveEvent += OnMove;
        InputController.fireEvent += OnFire;
    }

    protected override void RemoveListeners() {
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }

    protected virtual void OnMove(object sender, InfoEventArgs<Point> e) {

    }

    protected virtual void OnFire(object sender, InfoEventArgs<int> e) {

    }

    /// <summary>
    /// Assuming the level contains a tile at the specified point, sets the selected tile
    /// </summary>
    /// <param name="p">Point to check</param>
    protected virtual void SelectTile(Point p) {
        if (pos == p || !level.tiles.ContainsKey(p))
            return;

        pos = p;
        tileSelectionIndicator.localPosition = level.tiles[p].center;
    }
}
