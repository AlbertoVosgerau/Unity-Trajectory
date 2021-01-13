using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScenes2D
{
    public static Scene currentScene;
    public static PhysicsScene2D currenScenePhysics;

    public static Scene simulationScene;
    public static PhysicsScene2D simulationPhysicsScene;
    public static string simulationSceneName = "TrajectorySimulationScene";

    public static void InitializePhysicsScene2D(string currentScene)
    {
        RegisterCurrentScene(currentScene);
        SetSimulationScene();
    }

    public static void RegisterCurrentScene(string sceneName)
    {
        if (currentScene.name == sceneName)
            return;

        currentScene = SceneManager.GetSceneByName(sceneName);
        currenScenePhysics = currentScene.GetPhysicsScene2D();
    }

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
