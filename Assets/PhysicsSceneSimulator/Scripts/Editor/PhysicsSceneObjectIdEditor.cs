using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsSceneObjectId))]
public class PhysicsSceneObjectIdEditor : Editor
{
    PhysicsSceneObjectId id;

    private void OnEnable()
    {
        id = (PhysicsSceneObjectId)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label($"Object ID: {id.Id}");
        if(GUILayout.Button("Copy"))
        {
            EditorGUIUtility.systemCopyBuffer = id.Id;
        }
        EditorGUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}
