using Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Level;


[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    LevelManager manager;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        manager = (LevelManager)target;

        // Level Instancing
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Instance Full Mall"))
        {
            manager.InstanceElevatorShaft();
            manager.InstanceMall();
        }
        if (GUILayout.Button("Instance Elevator"))
        {
            manager.InstanceElevatorShaft();
        }
        if (GUILayout.Button("Instance Mall"))
        {
            manager.InstanceMall();
        }
        // Level Destruction
        EditorGUILayout.Space();
        if (GUILayout.Button("Destroy Full Level"))
        {
            manager.DeleteLevel(true);
        }
        if (GUILayout.Button("Destroy Level"))
        {
            manager.DeleteLevel();
        }
    }
}
