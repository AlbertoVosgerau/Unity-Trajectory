using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DCloneHandler : MonoBehaviour
{
    #region Public Variables
    public bool createCopyOnStart = true;
    public bool includeSimulationPhysics = true;
    public bool syncTransform = false;
    [HideInInspector]
    public List<GameObject> clones = new List<GameObject>();
    public List<MonoBehaviour> removeOnCopy;
    [HideInInspector]
    public PhysicsSceneObjectId id;
    #endregion

    #region Private Variables
    private string uniqueId;
    #endregion

    #region Monobehaviour
    private void Start()
    {
        PhysicsScenes2D.SetSimulationScene2D();
        uniqueId = UniqueId();
        id = gameObject.AddComponent<PhysicsSceneObjectId>();
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

        UpdateTransforms();        
    }
    private void OnDestroy()
    {
        if (!id.IsOriginal)
            return;

        Debug.Log($"Object on scene {gameObject.scene.name} is marked as original");

        DestroyAllClones();
    }
    #endregion

    #region Transform Updates
    public void UpdateTransforms()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            GameObject cloneObject = clones[i];
            if (cloneObject == null)
                break;

            cloneObject.transform.position = transform.position;
            cloneObject.transform.rotation = transform.rotation;
            cloneObject.transform.localScale = transform.localScale;
        }
    }
    #endregion

    #region Object Creation and Destruction

    public void SwapScene(Scene from, Scene to)
    {
        if (!id.IsOriginal)
            return;

        GameObject objectToMove = null;

        for (int i = 0; i < clones.Count; i++)
        {
            GameObject clone = clones[i];
            if(clone != null)
            {
                if (clone.scene == to)
                {
                    objectToMove = clones[i];
                    break;
                }
            }
        }

        if (objectToMove == null)
            return;

        SceneManager.MoveGameObjectToScene(gameObject, to);
        SceneManager.MoveGameObjectToScene(objectToMove, from);
    }
    public void CloneForAllScenes()
    {
        if(includeSimulationPhysics)
        {
            CreateCopy(PhysicsScenes2D.simulationScene);
        }
        for (int i = 0; i < PhysicsScenes2D.customScenes.Count; i++)
        {
            CreateCopy(PhysicsScenes2D.customScenes[i].scene);
        }

        if (gameObject.scene.name == PhysicsScenes2D.currentScene.name)
            return;

        CreateCopy(PhysicsScenes2D.currentScene);
    }
    public void DestroyAllClones()
    {
        //for (int i = 0; i < clones.Count; i++)
        //{
        //    DestroyCopy(clones[i]);
        //}
    }
    public void CreateCopy(Scene scene)
    {
        if (!scene.IsValid())
            return;

        GameObject cloneObject = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
        cloneObject.name = $"simulated_{gameObject.name}";

        ClearComponents(cloneObject);

        PhysicsScene2DCloneHandler cloneHandler = cloneObject.GetComponent<PhysicsScene2DCloneHandler>();
        cloneHandler.id.SetId(uniqueId);
        cloneHandler.id.SetIsOriginal(false);

        clones.Add(cloneObject);
        SceneManager.MoveGameObjectToScene(cloneObject, scene);
    }
    public void DestroyCopy(GameObject cloneObject)
    {
        clones.Remove(cloneObject);
        if (cloneObject == null)
            return;

        Destroy(cloneObject);
    }
    private void ClearComponents(GameObject cloneObject)
    {
        Renderer renderer = cloneObject.GetComponent<Renderer>();
        if (renderer != null)
            Destroy(renderer);
        Rigidbody2D rb = cloneObject.GetComponent<Rigidbody2D>();
        if (rb != null)
            Destroy(rb);
        MeshFilter meshFilter = cloneObject.GetComponent<MeshFilter>();
        if (meshFilter != null)
            Destroy(meshFilter);

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
    #endregion
}
