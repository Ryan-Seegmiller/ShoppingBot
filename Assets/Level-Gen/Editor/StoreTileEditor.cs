using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LevelGen;

[CustomEditor(typeof(StoreTile))]
public class StoreTileEditor : Editor
{
    StoreTile store;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        store = (StoreTile)target;

        // Level Instancing
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Spawn Items"))
        {
            store.SpawnItems();
        }
        if (GUILayout.Button("Destroy Items"))
        {
            store.DestroyItems();
        }
    }

    private void OnSceneGUI()
    {
        store = (StoreTile)target;
        Handles.color = Color.white;
        GUI.color = Color.blue;

        for (int i = 0; i < store.spawns.Length; i++)
        {
            Vector3 pos = store.transform.position + store.transform.InverseTransformVector(store.spawns[i]);
            Handles.DrawWireCube(pos, Vector3.one * .2f);
            Handles.Label(pos, "spawns[" + i + "]");
        }
    }

}