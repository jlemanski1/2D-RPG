using UnityEngine;

public class Tile : MonoBehaviour {

    // 4 steps = width of tile
    public const float stepHeight = 0.25f;

    public Point pos;
    public int height;

    public GameObject content;  // Entity on the tile e.g. Unit, Tree, Trap, etc.

    // Center of the tile
    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }

    [HideInInspector] public Tile prev;     // Tile that was traversed to reach the point
    [HideInInspector] public int distance;  // Number of tiles traversed to reach the point

    /// <summary>
    /// Visually reflect any new values
    /// Updates position & scale
    /// </summary>
    private void UpdateTile() {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    /// <summary>
    /// Increase tile size
    /// </summary>
    public void Grow() {
        height++;
        UpdateTile();
    }

    /// <summary>
    /// Decrease tile size
    /// </summary>
    public void Shrink() {
        height--;
        UpdateTile();
    }

    /// <summary>
    /// Loads Tile position and height for persisting tile data
    /// </summary>
    /// <param name="p">position</param>
    /// <param name="h">height</param>
    public void Load(Point p, int h) {
        pos = p;
        height = h;
        UpdateTile();
    }

    /// <summary>
    /// Loads Tile position through a Vector3
    /// </summary>
    /// <param name="v">Vector3 containing tile position</param>
    public void Load(Vector3 v) {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }

}
