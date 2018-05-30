using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Automatically copies the data from the spreadsheets to a scriptable object
/// Listens for changes in the data spreadsheets, and updates as necessary
/// </summary>
public class DataAutoConverter : AssetPostprocessor {

    static Dictionary<string, Action> parsers;

    /* To load/Unload assets at runtime
     * 
     * void Start() {
     * EnemyData test = Resources.Load<EnemyData>("TEST");      // Load
     * Debug.Log(test.hp);
     * test = null;
     * Resources.UnloadUnusedAssets();      // Unload
     * }
     */


    /// <summary>
    /// Link the names of data spreadsheets to an Action (Delegate that performs a task)
    /// </summary>
    static DataAutoConverter() {
        parsers = new Dictionary<string, Action>();
        parsers.Add("Enemies_test.csv", ParseEnemies);
        parsers.Add("Conversation_Test.csv", ParseDialogue);
    }


    /// <summary>
    /// Loop through changed files and if any of them are in the parser dictionary,
    /// parse the spreadsheet with the associated action
    /// </summary>
    static void OnPostProcessAllAssets(string[] importedAssets, string[] deletedAssets,
        string[] movedAssets, string[] movedFromAssetPaths) {
        for (int i = 0; i < importedAssets.Length; i++) {
            string fileName = Path.GetFileName(importedAssets[i]);
            if (parsers.ContainsKey(fileName))
                parsers[fileName]();
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// Parse the data and create the necessary ScriptableObject in the Data/Resources/ folder
    /// </summary>
    static void ParseEnemies() {
        string filePath = Application.dataPath + "/Data/Enemies_test.csv";
        if (!File.Exists(filePath)) {
            Debug.LogError("ERROR! Missing Enemy Data: " + filePath);
            return;
        }

        string[] readText = File.ReadAllLines("Assets/Data/Enemies_test.csv");
        filePath = "Assets/Data/Resources/";
        for (int i = 1; i < readText.Length; i++) {
            EnemyData enemyData = ScriptableObject.CreateInstance<EnemyData>();
            enemyData.Load(readText[1]);
            string fileName = string.Format("{0}{1}.asset", filePath, enemyData.name);
            AssetDatabase.CreateAsset(enemyData, fileName);
        }
    }


    /// <summary>
    /// Parse the conversation data and create the necessary Scriptable Object for the
    /// ConversationController to use
    /// </summary>
    static void ParseDialogue() {
        string filePath = Application.dataPath + "/Data/Conversation_Test.csv";
        if (!File.Exists(filePath)) {
            Debug.LogError("ERROR! Missing Conversation Data: " + filePath);
            return;
        }

        string[] readText = File.ReadAllLines("Assets/Data/Conversation_Test.csv");
        Debug.Log("readText: " + readText);
        filePath = "Assets/Data/Resources/";
        for (int i = 1; i < readText.Length; i++) {
            SpeakerData speakerData = ScriptableObject.CreateInstance<SpeakerData>();
            speakerData.Load(readText[1]);  // Load 1st line of actual data
            string fileName = string.Format("{0}{1}.asset", filePath, speakerData.speakerName);
            AssetDatabase.CreateAsset(speakerData, fileName);
        }
    }
}
