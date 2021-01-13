using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryProjectionSceneData
{
    public static Scene currentScene;
    public static PhysicsScene2D currenScenePhysics;

    public static Scene simulationScene;
    public static PhysicsScene2D simulationPhysicsScene;
    public static string simulationSceneName = "simulationScene";

    public static void SetSimulationScene()
    {
        if (simulationScene.name == simulationSceneName)
            return;

        CreateSceneParameters sceneParams = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        simulationScene = SceneManager.CreateScene(simulationSceneName, sceneParams);
        simulationPhysicsScene = simulationScene.GetPhysicsScene2D();
        simulationScene.name = simulationSceneName;
    }
}
