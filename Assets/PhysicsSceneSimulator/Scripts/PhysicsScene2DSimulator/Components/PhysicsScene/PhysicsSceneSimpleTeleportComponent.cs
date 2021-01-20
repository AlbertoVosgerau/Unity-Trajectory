using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSceneSimpleTeleportComponent : MonoBehaviour
{
    private PhysicsScene2DCloneHandler cloneHandler;
    public CustomPhysicsScene2DUpdater initialScene;
    private void Start()
    {
        StartCoroutine(StartRoutine()); 
    }
    private IEnumerator StartRoutine()
    {
        yield return new WaitForEndOfFrame();
        cloneHandler = GetComponent<PhysicsScene2DCloneHandler>();

        if (initialScene != null)
        {
            MoveToScene(SceneManager.GetSceneByName(initialScene.sceneName));
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
}
