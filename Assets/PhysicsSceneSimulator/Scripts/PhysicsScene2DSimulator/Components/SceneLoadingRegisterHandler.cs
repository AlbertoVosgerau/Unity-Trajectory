using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingRegisterHandler : MonoBehaviour
{
    #region Singleton
    public static SceneLoadingRegisterHandler Instance => _instance;
    private static SceneLoadingRegisterHandler _instance;
    #endregion
    #region Private Variables
    private List<CustomPhysicsScene2DUpdater> _CustomScenes = new List<CustomPhysicsScene2DUpdater>();
    #endregion

    #region Public Events
    public Action<float> onSceneLoading;
    public Action onSceneFinishedLoading;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this);
        SceneManager.activeSceneChanged += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene from, Scene to)
    {
        RegisterScene(to);
        RegisterCustomScenes();
    }
    #endregion

    #region Scene Registration
    public void RegisterScene(Scene scene)
    {
        PhysicsScenes2D.InitializePhysicsScene2D(scene.name);
    }
    public void RegisterCustomScenes()
    {
        _CustomScenes = FindObjectsOfType<CustomPhysicsScene2DUpdater>().ToList();

        for (int i = 0; i < _CustomScenes.Count; i++)
        {
            _CustomScenes[i].RegisterScene();
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
            AsyncOperation unloadCustomScene = SceneManager.UnloadSceneAsync(PhysicsScenes2D.customScenes[i].SceneName);

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

        if(onSceneFinishedLoading != null)
            onSceneFinishedLoading.Invoke();

        Resources.UnloadUnusedAssets();
    }
    #endregion
}