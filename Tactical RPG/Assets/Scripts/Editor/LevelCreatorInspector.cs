using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom Editor to allow for creation of levels during edit-time
/// </summary>

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorInspector : Editor {

    // Selected object
    public LevelCreator current {
        get { return (LevelCreator)target; }
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear"))
            current.ClearLevel();

        if (GUILayout.Button("Grow"))
            current.Grow();

        if (GUILayout.Button("Shirnk"))
            current.Shrink();

        if (GUILayout.Button("Grow Area"))
            current.GrowArea();

        if (GUILayout.Button("Shrink Area"))
            current.ShrinkArea();

        if (GUILayout.Button("Save"))
            current.SaveLevel();

        if (GUILayout.Button("Load"))
            current.LoadLevel();

        if (GUI.changed)
            current.UpdateMarker();
    }
}
