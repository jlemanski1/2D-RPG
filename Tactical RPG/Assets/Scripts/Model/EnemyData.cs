using System;
using UnityEngine;

/// <summary>
/// Loads and holds an entry of the enemy data spreadsheet
/// </summary>
[System.Serializable]
public class EnemyData : ScriptableObject {

    public int hp;          // Health points
    public int hitCount;    // Amount of times it can attack
    public int damage;      // Attack damage
    public int agility;     // Agility / Weight (speed/heft)
    public int xp;          // XP reward for killing
    public int gold;        // Gold reward for killing


    /// <summary>
    /// Loads enemy data file and saves to appropriate vars
    /// </summary>
    public void Load(string line) {
        string[] elements = line.Split(',');
        name = elements[0];
        hp = Convert.ToInt32(elements[1]);
        hitCount = Convert.ToInt32(elements[2]);
        damage = Convert.ToInt32(elements[3]);
        agility = Convert.ToInt32(elements[4]);
        xp = Convert.ToInt32(elements[5]);
        gold = Convert.ToInt32(elements[6]);
    }
}
