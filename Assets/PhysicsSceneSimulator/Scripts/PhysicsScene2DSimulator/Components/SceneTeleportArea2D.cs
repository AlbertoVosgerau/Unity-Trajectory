using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleportArea2D : MonoBehaviour
{
    public CustomPhysicsScene2DUpdater onEnterScene;
    public CustomPhysicsScene2DUpdater onExitScene;

    private PhysicsSceneObjectId id;

    private void Start()
    {
        id = GetComponent<PhysicsSceneObjectId>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhysicsScene2DTeleportComponent sceneSwitcher = collision.gameObject.GetComponent<PhysicsScene2DTeleportComponent>();

        if (sceneSwitcher == null || id == null)
            return;

        if (sceneSwitcher.isOnTriggerArea && id.Id == sceneSwitcher.storedId)
            return;

        sceneSwitcher.isOnTriggerArea = true;
        sceneSwitcher.storedId = id.Id;

        sceneSwitcher.StoreRigidbodyData();
        if (onExitScene == null)
            MoveToScene(PhysicsScenes2D.currentScene.name);
        else
            MoveToScene(onExitScene.name);
        sceneSwitcher.RestoreRigidbodyData();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PhysicsScene2DTeleportComponent sceneSwitcher = collision.gameObject.GetComponent<PhysicsScene2DTeleportComponent>();

        if (sceneSwitcher == null)
            return;

        if (sceneSwitcher.isOnTriggerArea && id.Id == sceneSwitcher.storedId)
            return;

        sceneSwitcher.isOnTriggerArea = false;

        sceneSwitcher.StoreRigidbodyData();
        if (onEnterScene == null)
            MoveToScene(PhysicsScenes2D.currentScene.name);
        else
            MoveToScene(onEnterScene.name);
        sceneSwitcher.RestoreRigidbodyData();
    }

    private void MoveToScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return;

        int index = PhysicsScenes2D.CustomScene2DIndex(sceneName);
        SceneManager.MoveGameObjectToScene(gameObject, PhysicsScenes2D.customScenes[index].scene);
    }
}
