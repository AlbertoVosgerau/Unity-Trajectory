using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSceneCloneHandler : MonoBehaviour
{
    public bool createCopyOnStart = true;
    private GameObject cloneObject;
    private void Start()
    {
        TrajectoryProjectionSceneData.SetSimulationScene();

        if(createCopyOnStart)
            CreateCopy();
    }
    public void CreateCopy()
    {
        cloneObject = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
        cloneObject.name = $"simulated_{gameObject.name}";
        Destroy(cloneObject.GetComponent<PhysicsSceneCloneHandler>());
        Debug.Log("Move");
        SceneManager.MoveGameObjectToScene(cloneObject, TrajectoryProjectionSceneData.simulationScene);
    }
    public void DestroyCopy()
    {
        Destroy(cloneObject);
    }
}
