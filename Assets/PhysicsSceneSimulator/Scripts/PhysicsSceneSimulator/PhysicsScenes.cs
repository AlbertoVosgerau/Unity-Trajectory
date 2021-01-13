using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScenes : MonoBehaviour
{
    public static Scene currentScene;
    public static PhysicsScene currenScenePhysics;

    public static Scene simulationScene;
    public static PhysicsScene simulationPhysicsScene;
    public static string simulationSceneName = "TrajectorySimulationScene";

    public static void InitializePhysicsScene(string currentScene)
    {
        RegisterCurrentScene(currentScene);
        SetSimulationScene();
    }

    public static void RegisterCurrentScene(string sceneName)
    {
        if (currentScene.name == sceneName)
            return;

        currentScene = SceneManager.GetSceneByName(sceneName);
        currenScenePhysics = currentScene.GetPhysicsScene();
    }

    public static void SetSimulationScene()
    {
        if (simulationScene.name == simulationSceneName)
            return;

        CreateSceneParameters sceneParams = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        simulationScene = SceneManager.CreateScene(simulationSceneName, sceneParams);
        simulationPhysicsScene = simulationScene.GetPhysicsScene();
        simulationScene.name = simulationSceneName;
    }
}
