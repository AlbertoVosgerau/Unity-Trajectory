using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DCloneHandler : MonoBehaviour
{
    public bool createCopyOnStart = true;
    public bool syncTransform = false;
    public List<MonoBehaviour> removeOnCopy;
    private List<GameObject> clones = new List<GameObject>();
    private string uniqueId;
    private void Start()
    {
        PhysicsScenes2D.SetSimulationScene();
        uniqueId = UniqueId();
        PhysicsSceneObjectId id = gameObject.AddComponent<PhysicsSceneObjectId>();
        id.SetId(uniqueId);

        if (createCopyOnStart)
        {
            CloneForAllScenes();
        }

    }
    private void Update()
    {
        if (!syncTransform || clones.Count == 0)
            return;

        for (int i = 0; i < clones.Count; i++)
        {
            GameObject cloneObject = clones[i];
            cloneObject.transform.position = transform.position;
            cloneObject.transform.rotation = transform.rotation;
            cloneObject.transform.localScale = transform.localScale;
        }
    }
    
    public void CloneForAllScenes()
    {
        CreateCopy(PhysicsScenes2D.simulationScene);
        for (int i = 0; i < PhysicsScenes2D.customScenes.Count; i++)
        {
            CreateCopy(PhysicsScenes2D.customScenes[i].scene);
        }
    }
    public void CreateCopy(Scene scene)
    {
        GameObject cloneObject = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
        cloneObject.name = $"simulated_{gameObject.name}";

        ClearComponents(cloneObject);

        PhysicsSceneObjectId id = cloneObject.AddComponent<PhysicsSceneObjectId>();
        id.SetId(uniqueId);

        clones.Add(cloneObject);
        SceneManager.MoveGameObjectToScene(cloneObject, scene);
    }
    public void DestroyCopy(GameObject cloneObject)
    {
        if (cloneObject == null)
            return;

        Destroy(cloneObject);
    }

    private void ClearComponents(GameObject cloneObject)
    {
        Renderer renderer = cloneObject.GetComponent<Renderer>();
        if (renderer != null)
            Destroy(renderer);

        PhysicsScene2DCloneHandler cloneHandler = cloneObject.GetComponent<PhysicsScene2DCloneHandler>();
        for (int i = 0; i < removeOnCopy.Count; i++)
        {
            Destroy(cloneHandler.removeOnCopy[i]);
        }
        Destroy(cloneHandler);
    }

    private string UniqueId()
    {
        Guid guid = Guid.NewGuid();
        string str = guid.ToString();
        return str;
    }
}
