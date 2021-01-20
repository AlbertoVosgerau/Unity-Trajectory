using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleportArea2D : MonoBehaviour
{
    public CustomPhysicsScene2DUpdater onEnterScene;
    public CustomPhysicsScene2DUpdater onExitScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhysicsScene2DTeleportComponent sceneSwitcher = collision.gameObject.GetComponent<PhysicsScene2DTeleportComponent>();

        if (sceneSwitcher == null)
            return;

        sceneSwitcher.StoreRigidbodyData();
        if (onEnterScene == null)
            MoveToScene(sceneSwitcher, collision.gameObject, PhysicsScenes2D.currentScene.name);
        else
            MoveToScene(sceneSwitcher, collision.gameObject, onEnterScene.sceneName);
        sceneSwitcher.RestoreRigidbodyData();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PhysicsScene2DTeleportComponent sceneSwitcher = collision.gameObject.GetComponent<PhysicsScene2DTeleportComponent>();

        if (sceneSwitcher == null)
            return;

        if (sceneSwitcher.ignoreNextExit)
        {
            sceneSwitcher.ignoreNextExit = !sceneSwitcher.ignoreNextExit;
            return;
        }

        sceneSwitcher.ignoreNextExit = true;

        sceneSwitcher.StoreRigidbodyData();
        if (onExitScene == null)
            MoveToScene(sceneSwitcher, collision.gameObject, PhysicsScenes2D.currentScene.name);
        else
            MoveToScene(sceneSwitcher, collision.gameObject, onExitScene.sceneName);        
        sceneSwitcher.RestoreRigidbodyData();
    }

    private void MoveToScene(PhysicsScene2DTeleportComponent sceneSwitcher, GameObject objectToTeleport, string sceneName)
    {
        if(sceneName == PhysicsScenes2D.currentScene.name)
        {
            //SceneManager.MoveGameObjectToScene(objectToTeleport, PhysicsScenes2D.currentScene);
            sceneSwitcher.SwapScenes(gameObject.scene, PhysicsScenes2D.currentScene);
            return;
        }

        int index = PhysicsScenes2D.CustomScene2DIndex(sceneName);
        if (!PhysicsScenes2D.customScenes[index].scene.IsValid())
            return;

        //SceneManager.MoveGameObjectToScene(objectToTeleport, PhysicsScenes2D.customScenes[index].scene);
        sceneSwitcher.SwapScenes(gameObject.scene, PhysicsScenes2D.customScenes[index].scene);
    }
}
