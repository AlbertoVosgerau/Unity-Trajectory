using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomPhysicsScene2DUpdater))]
public class CustomPhysicsScene2DUpdaterEditor : Editor
{
    CustomPhysicsScene2DUpdater updater;
    private void OnEnable()
    {
        updater = (CustomPhysicsScene2DUpdater)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        string customName = $"Scene: {updater.SceneName} - {updater.timeScaleType.ToString()} {updater.timeIterations}x";
        if (updater.name != customName)
            updater.name = customName;
    }
}
