using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    [SerializeField] GameObject tilePrefab;

    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    /// <summary>
    /// Loads from a LevelData and creates the level
    /// </summary>
    /// <param name="data">LevelData to load from</param>
    public void Load(LevelData data) {
        for (int i = 0; i < data.tiles.Count; i++) {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }
}
