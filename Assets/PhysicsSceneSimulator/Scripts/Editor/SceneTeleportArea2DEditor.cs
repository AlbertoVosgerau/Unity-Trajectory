using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomSceneTeleportArea2D))]
public class SceneTeleportArea2DEditor : Editor
{
    CustomSceneTeleportArea2D teleporter;
    private void OnEnable()
    {
        teleporter = (CustomSceneTeleportArea2D)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string onEnter = teleporter.onEnterScene == null ? "Current" : teleporter.onEnterScene.SceneName;
        string onExit = teleporter.onExitScene == null? "Current" : teleporter.onExitScene.SceneName;
        string customName = $"SceneTeleport - OnEnter: {onEnter} | OnExit: {onExit}";
        if (teleporter.name != customName)
            teleporter.name = customName;
    }
}
