using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeakerData : ScriptableObject {

    public int id;                  // Message ID
    public string[] messages;   // Sequential messages spoken
    public Sprite speaker;      // sprite showing the character that's speaking
    public string speakerName;  // Name of character currently speaking
    public TextAnchor anchor;   // Corner to anchor the panel on


    /// <summary>
    /// Loads conversation data file and saves to appropriate vars
    /// </summary>
    public void Load(string line) {
        string[] elements = line.Split(',');
        id = Convert.ToInt32(elements[0]);
        speakerName = elements[1];
        messages = elements[2].Split('\n');
    }
}
