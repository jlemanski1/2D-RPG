using System.Collections;
using UnityEngine;

public static class DirectionsExtension {

    /// <summary>
    /// Returns a cardinal direction based off the relationship between two tiles
    /// Example: Directions dir = t1.GetDirections(t2);
    /// </summary>
    /// <param name="t1">Tile 1</param>
    /// <param name="t2">Tile 2</param>
    /// <returns></returns>
    public static Directions GetDirection(this Tile t1, Tile t2) {
        if (t1.pos.y < t2.pos.y)
            return Directions.North;
        if (t1.pos.x < t2.pos.x)
            return Directions.East;
        if (t1.pos.y > t2.pos.y)
            return Directions.South;
        return Directions.West;
    }

    /// <summary>
    /// Converts Direction to Euler angle
    /// Example: 
    ///     Directions dir = Directions.North;
    ///     Vector3 r = dir.ToEuler();
    /// </summary>
    /// <param name="dir">Direction to convert</param>
    /// <returns></returns>
    public static Vector3 ToEuler(this Directions dir) {
        return new Vector3(0, (int)dir * 90, 0);
    }
	
}
