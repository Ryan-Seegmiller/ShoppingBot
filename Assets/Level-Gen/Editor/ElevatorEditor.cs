using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Level;

[CustomEditor(typeof(Elevator))]
public class ElevatorEditor : Editor
{
    Elevator elevator;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        elevator = (Elevator)target;

        if (GUILayout.Button("Open"))
        {
            elevator.DoorTrigger();
        }
        if (GUILayout.Button("Call"))
        {
            elevator.Trigger();
        }
    }
}
