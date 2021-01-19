using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRegisterHandler : MonoBehaviour
{
    private List<CustomPhysicsScene2DUpdater> customScenes = new List<CustomPhysicsScene2DUpdater>();
    private void Awake()
    {
        RegisterScene();
        RegisterCustomScenes();
    }
    public void RegisterScene()
    {
        PhysicsScenes2D.InitializePhysicsScene2D(SceneManager.GetActiveScene().name);
    }
    public void RegisterCustomScenes()
    {
        customScenes = FindObjectsOfType<CustomPhysicsScene2DUpdater>().ToList();

        for (int i = 0; i < customScenes.Count; i++)
        {
            customScenes[i].RegisterScene();
        }
    }
    public void LoadNewScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }
    private IEnumerator LoadScene(string sceneName)
    {
        for (int i = PhysicsScenes2D.customScenes.Count-1; i >= 0; i--)
        {
            AsyncOperation unloadCustomScene = SceneManager.UnloadSceneAsync(PhysicsScenes2D.customScenes[i].sceneName);

            while (!unloadCustomScene.isDone)
            {
                yield return null;
            }
            Debug.Log($"Unregistered scene {PhysicsScenes2D.customScenes[i].sceneName}");
            PhysicsScenes2D.UnregisterScene2D(i);
        }
        AsyncOperation unloadSimulationScene = SceneManager.UnloadSceneAsync(PhysicsScenes2D.simulationSceneName);

        while (!unloadSimulationScene.isDone)
        {
            yield return null;
        }
        Debug.Log("Unloaded simulation scene");

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName);
        loadScene.allowSceneActivation = true;
        while (!loadScene.isDone)
        {
            yield return null;
        }
        Debug.Log("Loaded new scene");

        Resources.UnloadUnusedAssets();
    }
}
