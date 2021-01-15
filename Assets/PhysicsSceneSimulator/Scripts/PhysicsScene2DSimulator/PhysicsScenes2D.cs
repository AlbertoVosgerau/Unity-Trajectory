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

    public static List<CustomPhysicsScene2D> customScenes = new List<CustomPhysicsScene2D>();

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

    public static int RegisterNewScene2D(string sceneName)
    {
        CustomPhysicsScene2D sceneToRegister = new CustomPhysicsScene2D(sceneName);
        customScenes.Add(sceneToRegister);
        return customScenes.IndexOf(sceneToRegister);
    }

    public static void UnregisterScene2D(int index)
    {
        CustomPhysicsScene2D sceneToUnregister = customScenes[index];
        customScenes.Remove(sceneToUnregister);
    }

    public static int CustomScene2DIndex(string sceneName)
    {
        for (int i = 0; i < customScenes.Count; i++)
        {
            Debug.Log($"Found scene: {customScenes[i].sceneName} looking for {sceneName}");
        }
        int index = customScenes.FindIndex(item => item.sceneName == sceneName);
        if (index < 0)
            index = 0;

        Debug.Log(index);
        return index;
    }
}
