using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    [SerializeField] GameObject tilePrefab;

    // Array of points for each direction
    private Point[] dirs = new Point[4] {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    // Tile Colours
    Color selectedTileColour = new Color(0, 1, 1, 1);
    Color defaultTileColour = new Color(1, 1, 1, 1);

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

    /// <summary>
    /// Return a list of Tiles
    /// </summary>
    /// <param name="start"> Starting Tile</param>
    /// <param name="addTile"></param>
    /// <returns></returns>
    public List<Tile> Search (Tile start, Func<Tile, Tile, bool> addTile) {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        // Prime the search
        start.distance = 0;
        checkNow.Enqueue(start);

        // Main Search loop
        while (checkNow.Count > 0) {
            Tile t = checkNow.Dequeue();
            // Get the tiles in each direction from the selected tile
            for (int i = 0; i < 4; i++) {   // 4 =  dirs.Length
                Tile next = GetTile(t.pos + dirs[i]);

                // Check the next is actually a Tile
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                // Double check Tile criteria
                if (addTile(t, next)) {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }
            // Check if the queue is cleared
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return retValue;
    }


    /// <summary>
    /// Swaps Tile Refrences around
    /// </summary>
    /// <param name="a">Tile A</param>
    /// <param name="b">Tile B</param>
    private void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b) {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }


    /// <summary>
    /// Returns a Tile based off the given point
    /// </summary>
    /// <param name="p">Point to search</param>
    /// <returns></returns>
    public Tile GetTile(Point p) {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }


    /// <summary>
    /// Clear results of any previous search
    /// </summary>
    void ClearSearch() {
        foreach (Tile t in tiles.Values) {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }


    /// <summary>
    /// Highlights selected Tiles by changing their mat colour
    /// </summary>
    /// <param name="tiles"></param>
    public void SelectTiles(List<Tile> tiles) {
        for (int i = tiles.Count - 1; i >= 0; i--) {
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColour);
        }
    }


    /// <summary>
    /// Sets Tile colour back to default
    /// </summary>
    /// <param name="tiles"></param>
    public void DeSelectTiles(List<Tile> tiles) {
        for (int i = tiles.Count - 1; i >= 0; i--) {
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColour);
        }
    }


}
