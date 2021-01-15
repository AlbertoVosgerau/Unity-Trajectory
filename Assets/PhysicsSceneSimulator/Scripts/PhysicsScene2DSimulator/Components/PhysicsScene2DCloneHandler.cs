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
    private GameObject cloneObject;
    private string uniqueId;
    private void Start()
    {
        PhysicsScenes2D.SetSimulationScene();

        if(createCopyOnStart)
        {
            CloneForAllScenes();
        }

        uniqueId = UniqueId();
    }
    private void Update()
    {
        if (cloneObject == null)
            return;

        if (!syncTransform || cloneObject == null)
            return;

        cloneObject.transform.position = transform.position;
        cloneObject.transform.rotation = transform.rotation;
        cloneObject.transform.localScale = transform.localScale;
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
        cloneObject = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
        cloneObject.name = $"simulated_{gameObject.name}";

        ClearComponents();

        PhysicsSceneObjectId id = cloneObject.AddComponent<PhysicsSceneObjectId>();
        id.SetId(UniqueId());

        SceneManager.MoveGameObjectToScene(cloneObject, scene);
    }
    public void DestroyCopy()
    {
        if (cloneObject == null)
            return;

        Destroy(cloneObject);
    }

    private void ClearComponents()
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
