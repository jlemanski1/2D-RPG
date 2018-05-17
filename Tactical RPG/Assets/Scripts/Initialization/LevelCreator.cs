using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelCreator : MonoBehaviour {

    [SerializeField] GameObject tileViewPrefab;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;

    // Max Level Extents
    [SerializeField] int maxWidth = 10;
    [SerializeField] int maxDepth = 10;
    [SerializeField] int maxHeight = 8;

    [SerializeField] Point pos;     // To modify individual tile
    [SerializeField] LevelData levelData;   // For loading previously saved levels to later edit

    #region Lazy Loading Pattern

    Transform marker {
        get {
            if (_marker == null) {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }

    Transform _marker;

    #endregion

    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    /// <summary>
    /// Grow random area for level generation
    /// </summary>
    public void GrowArea() {
        Rect r = RandomRect();
        GrowRect(r);
    }

    /// <summary>
    /// Shrink rnadom area for level generation
    /// </summary>
    public void ShrinkArea() {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    /// <summary>
    /// Random Rect sized between 0 and the set max extents
    /// </summary>
    /// <returns>new Rect</returns>
    Rect RandomRect() {
        int x = UnityEngine.Random.Range(0, maxWidth);
        int y = UnityEngine.Random.Range(0, maxDepth);
        int w = UnityEngine.Random.Range(1, maxWidth - x + 1);
        int h = UnityEngine.Random.Range(1, maxWidth - y + 1);
        return new Rect(x, y, w, h);
    }

    /// <summary>
    /// Loop through the range of positions specified by the random rect area, growing
    /// a specific Tile at a time
    /// </summary>
    /// <param name="rect"> randomly generated rect area</param>
    void GrowRect(Rect rect) {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y) {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x) {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    /// <summary>
    /// Loop through the range of positions specified by the random rect area, shrinking
    /// a specific Tile at a time
    /// </summary>
    /// <param name="rect"> randomly generated rect area</param>
    void ShrinkRect(Rect rect) {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y) {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x) {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }
    
    /// <summary>
    /// Create Tile
    /// </summary>
    /// <returns>Tile</returns>
    Tile Create() {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    Tile GetOrCreate(Point p) {
        // Tile exists
        if (tiles.ContainsKey(p)) {
            return tiles[p];
        }

        // Tile does not exist, create one
        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);    // Add to tile dictionary

        return t;
    }

    /// <summary>
    /// Grow a single tile at a time
    /// </summary>
    /// <param name="p">Point corresponding to the tile to grow</param>
    private void GrowSingle(Point p) {
        Tile t = GetOrCreate(p);
        // Height is within level limits
        if (t.height < maxHeight) {
            t.Grow();
        }
    }

    /// <summary>
    /// Shrink a single tile at a time
    /// </summary>
    /// <param name="p">Point corresponding to the tile to shrink</param>
    private void ShrinkSingle(Point p) {
        // Return if tile doesn't exist, no need to create since its shrinking
        if (!tiles.ContainsKey(p))
            return;

        // Tile exists, shrink
        Tile t = tiles[p];
        t.Shrink();

        // Tile's too low, remove...
        if (t.height <= 0) {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    /// <summary>
    /// For modifying a single tile based on the Point's pos
    /// </summary>
    public void Grow() {
        GrowSingle(pos);
    }

    /// <summary>
    /// For modifying a single tile based on the Point's pos
    /// </summary>
    public void Shrink() {
        ShrinkSingle(pos);
    }

    /// <summary>
    /// Updates the position of the Tile Selection Indicator to match the selected Tile
    /// </summary>
    public void UpdateMarker() {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    /// <summary>
    /// Loops through all Tiles and destroys them, cleaning the dictionary of references
    /// </summary>
    public void ClearLevel() {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);

        tiles.Clear();
    }

    /// <summary>
    /// Saves each of the tiles' position and height data in a list of Vector3s to persist
    /// data between sessions. LevelData will be a scriptableobject
    /// </summary>
    public void SaveLevel() {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData level = ScriptableObject.CreateInstance<LevelData>();

        // Create List of Vector3s and set corresponding values for each tile
        level.tiles = new List<Vector3>(tiles.Count);
        foreach (Tile t in tiles.Values)
            level.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));

        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(level, fileName);
    }

    /// <summary>
    /// Creates applicable directories if non exist
    /// </summary>
    private void CreateSaveDirectory() {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");

        filePath += "/Levels";

        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Restores a previously saved LevelData
    /// </summary>
    public void LoadLevel() {
        ClearLevel();
        if (levelData == null)
            return;
         
        // Loop through each tilea
        foreach(Vector3 v in levelData.tiles) {
            Tile t = Create();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }
}
