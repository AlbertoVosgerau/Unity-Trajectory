using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsSceneContextMenu : Editor
{
    [MenuItem("GameObject/Physics Scene Simulator/Create Simluation Scene 2D", false, 0)]
    public static void CreateSimulationScene2D()
    {
        GameObject newObject = new GameObject();
        newObject.AddComponent<SimulationPhysicScene2DUpdater>();
        newObject.name = "Simulation Scene: SpeedUp 1x";
    }

    [MenuItem("GameObject/Physics Scene Simulator/Create Custom Scene 2D", false, 0)]
    public static void CreateCustomScene2D()
    {
        GameObject newObject = new GameObject();
        newObject.AddComponent<CustomPhysicsScene2DUpdater>();
        newObject.name = "Custom Scene: SpeedUp 1x";
    }
}
