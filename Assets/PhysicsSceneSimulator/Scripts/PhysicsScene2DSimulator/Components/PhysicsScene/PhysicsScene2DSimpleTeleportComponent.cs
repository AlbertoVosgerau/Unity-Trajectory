using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DSimpleTeleportComponent : MonoBehaviour
{
    #region Private Variables
    private PhysicsScene2DCloneHandler cloneHandler;
    #endregion

    #region Serialzed Fields
    [SerializeField] private CustomPhysicsScene2DUpdater _InitialScene;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        StartCoroutine(StartRoutine()); 
    }
    #endregion

    #region Move Scenes
    private IEnumerator StartRoutine()
    {
        yield return new WaitForEndOfFrame();
        cloneHandler = GetComponent<PhysicsScene2DCloneHandler>();

        if (_InitialScene != null)
        {
            MoveToScene(SceneManager.GetSceneByName(_InitialScene.SceneName));
        }
    }
    public void MoveToScene(Scene targetScene)
    {
        cloneHandler.SwapScene(gameObject.scene, targetScene);
    }
    public void MoveToMainScene()
    {
        cloneHandler.SwapScene(gameObject.scene, PhysicsScenes2D.currentScene);
    }
    #endregion
}
