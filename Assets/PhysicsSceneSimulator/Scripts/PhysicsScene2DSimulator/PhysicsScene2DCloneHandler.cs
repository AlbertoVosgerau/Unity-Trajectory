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
    private void Start()
    {
        PhysicsScenes2D.SetSimulationScene();

        if(createCopyOnStart)
            CreateCopy();
    }
    private void Update()
    {
        if (!syncTransform || cloneObject == null)
            return;

        cloneObject.transform.position = transform.position;
        cloneObject.transform.rotation = transform.rotation;
        cloneObject.transform.localScale = transform.localScale;
    }
    public void CreateCopy()
    {
        cloneObject = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
        cloneObject.name = $"simulated_{gameObject.name}";

        Renderer renderer = cloneObject.GetComponent<Renderer>();
        if (renderer != null)
            Destroy(renderer);

        PhysicsScene2DCloneHandler cloneHandler = cloneObject.GetComponent<PhysicsScene2DCloneHandler>();
        for (int i = 0; i < removeOnCopy.Count; i++)
        {
            Destroy(cloneHandler.removeOnCopy[i]);
        }
        Destroy(cloneHandler);

        SceneManager.MoveGameObjectToScene(cloneObject, PhysicsScenes2D.simulationScene);
    }
    public void DestroyCopy()
    {
        Destroy(cloneObject);
    }
}
