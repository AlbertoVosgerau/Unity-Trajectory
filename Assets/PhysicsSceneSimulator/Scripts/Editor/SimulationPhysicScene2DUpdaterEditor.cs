using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimulationPhysicScene2DUpdater))]
public class SimulationPhysicScene2DUpdaterEditor : Editor
{
    SimulationPhysicScene2DUpdater updater;
    private void OnEnable()
    {
        updater = (SimulationPhysicScene2DUpdater)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        string customName = $"Simulation Scene: {updater.timeScaleType.ToString()} {updater.timeIterations}x";
        if (updater.name != customName)
            updater.name = customName;
    }
}
