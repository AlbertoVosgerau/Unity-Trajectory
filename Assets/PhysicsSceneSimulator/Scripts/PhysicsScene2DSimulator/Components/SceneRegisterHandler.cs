using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRegisterHandler : MonoBehaviour
{
    #region Private Variables
    private List<CustomPhysicsScene2DUpdater> customScenes = new List<CustomPhysicsScene2DUpdater>();
    #endregion

    #region Public Events
    public Action<float> onSceneLoading;
    public Action onSceneFinishedLoading;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        RegisterScene();
        RegisterCustomScenes();
    }
    #endregion

    #region Scene Registration
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
    #endregion

    #region Scene Loading
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
            PhysicsScenes2D.UnregisterScene2D(i);
        }
        AsyncOperation unloadSimulationScene = SceneManager.UnloadSceneAsync(PhysicsScenes2D.simulationSceneName);

        while (!unloadSimulationScene.isDone)
        {
            yield return null;
        }

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName);
        while (!loadScene.isDone)
        {
            if(onSceneLoading != null)
                onSceneLoading.Invoke(loadScene.progress);
            yield return null;
        }
        loadScene.allowSceneActivation = true;
        onSceneFinishedLoading.Invoke();

        Resources.UnloadUnusedAssets();
    }
    #endregion
}