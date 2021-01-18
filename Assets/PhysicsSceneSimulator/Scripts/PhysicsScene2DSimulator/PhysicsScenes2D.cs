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

    private Coroutine unloadScenes;

    public static void InitializePhysicsScene2D(string currentScene)
    {
        RegisterCurrentScene(currentScene);
        SetSimulationScene();
        // TODO Unlad UnusedAsssets after unload
    }

    public static void RegisterCurrentScene(string sceneName)
    {
        if (currentScene.name == sceneName)
            return;

        Debug.Log($"Registered current scene to {sceneName}");

        currentScene = SceneManager.GetSceneByName(sceneName);
        currenScenePhysics = currentScene.GetPhysicsScene2D();
    }

    public static void SetSimulationScene()
    {
        if (simulationScene.name == simulationSceneName)
            return;

        Debug.Log($"Registered simulation scene to {simulationSceneName}");

        CreateSceneParameters sceneParams = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        simulationScene = SceneManager.CreateScene(simulationSceneName, sceneParams);
        simulationPhysicsScene = simulationScene.GetPhysicsScene2D();
        simulationScene.name = simulationSceneName;
    }

    public static int RegisterNewScene2D(string sceneName)
    {
        Debug.Log($"Registered custom scene {sceneName}");
        CustomPhysicsScene2D sceneToRegister = new CustomPhysicsScene2D(sceneName);
        customScenes.Add(sceneToRegister);
        return customScenes.IndexOf(sceneToRegister);
    }

    public static void UnregisterScene2D(int index)
    {
        CustomPhysicsScene2D sceneToUnregister = customScenes[index];
        SceneManager.UnloadSceneAsync(sceneToUnregister.scene);
        customScenes.Remove(sceneToUnregister);
        return;

        if (customScenes == null)
            return;

        if (customScenes[index] == null)
            return;
        sceneToUnregister = customScenes[index];
        customScenes.Remove(sceneToUnregister);
        SceneManager.UnloadSceneAsync(sceneToUnregister.scene);
    }

    public static void UnregisterAll()
    {
        UnregisterSimulationScene();

        for (int i = 0; i < customScenes.Count; i++)
        {
            UnregisterScene2D(i);
        }
    }

    private static void UnregisterSimulationScene()
    {
        SceneManager.UnloadSceneAsync(simulationScene);
    }

    public static int CustomScene2DIndex(string sceneName)
    {
        int index = customScenes.FindIndex(item => item.sceneName == sceneName);
        return index;
    }
}
