using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(CameraFollow))]
public class PlayerCamEditor : Editor
{

    CameraFollow m_CameraFollow;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        m_CameraFollow = target as CameraFollow;

        m_CameraFollow.SetCameraPosition();

        EditorGUI.BeginChangeCheck();

        m_CameraFollow.camAngle = EditorGUILayout.Slider("Camera angle", m_CameraFollow.camAngle, 0, 60);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RegisterCompleteObjectUndo(target, "Changed angle");
        }
        
    }

}
#endif
