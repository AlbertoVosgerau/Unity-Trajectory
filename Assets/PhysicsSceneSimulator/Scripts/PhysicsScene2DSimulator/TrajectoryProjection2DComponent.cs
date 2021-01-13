using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TrajectoryProjection2DComponent : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private bool fireAcionOnProjectionFinish = false;
    [SerializeField] private bool destroyProjectionOnFinish = false;
    [SerializeField] private bool hideRendererOnSimulation = true;
    [SerializeField] private List<MonoBehaviour> removeOnCopy;
    #endregion

    #region Unity Events
    public UnityEvent<Rigidbody2D> onPhysicsAction;
    public UnityEvent<Transform, Transform> onVisualize;
    public UnityEvent onSimulationFinished;
    #endregion

    #region Private Variables
    private GameObject simulationContainer;
    private GameObject simObject;
    private bool isOnSimulation;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        PhysicsScenes2D.InitializePhysicsScene2D(SceneManager.GetActiveScene().name);        
    }
    private void OnDestroy()
    {
        ClearProjection();
    }
    #endregion

    #region Physics Action
    public void FireAction()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        onPhysicsAction.Invoke(rb);
    }
    #endregion

    #region Simulation
    public void Simulate()
    {
        if (isOnSimulation)
            CancelSimulation();

        if (simulationContainer != null)
            ClearProjection();

        simObject = SimObject();
        Rigidbody2D rb = simObject.GetComponent<Rigidbody2D>();
        onPhysicsAction.Invoke(rb);

        simulationContainer = new GameObject($"simulationContainer_{gameObject.name}");

        StartCoroutine(SimulationLoop());        
    }
    public void CancelSimulation()
    {
        if (!isOnSimulation)
            return;

        isOnSimulation = false;
        Destroy(simObject);
        ClearProjection();
    }
    public void ClearProjection()
    {
        Destroy(simulationContainer);
    }
    private void OnSimulationFinished()
    {
        if (!isOnSimulation)
            return;

        isOnSimulation = false;

        Destroy(simObject);

        if (destroyProjectionOnFinish)
            ClearProjection();

        onSimulationFinished.Invoke();

        if (fireAcionOnProjectionFinish)
            FireAction();
    }
    private IEnumerator SimulationLoop()
    {
        int count = 0;
        isOnSimulation = true;
        while (count < 500 && isOnSimulation)
        {
            onVisualize.Invoke(simObject.transform, simulationContainer.transform);
            count++;
            yield return null;
        }
        OnSimulationFinished();
        yield return null;
    }
    #endregion

    #region Simulation GameObject
    private GameObject SimObject()
    {
        GameObject simObject = Instantiate(gameObject, transform.position, transform.rotation);
        simObject.name = $"virtual_{gameObject.name}";
        if (hideRendererOnSimulation)
        {
            Renderer renderer = simObject.GetComponent<Renderer>();
            Destroy(renderer);
        }

        ClearComponents();

        MoveObjectToSimulationScene(simObject);
        return simObject;
    }
    private void MoveObjectToSimulationScene(GameObject objectToMove)
    {
        SceneManager.MoveGameObjectToScene(objectToMove, PhysicsScenes2D.simulationScene);
    }
    private void ClearComponents()
    {
        if (simObject == null)
            return;

        TrajectoryProjection2DComponent trajectoryComponent = simObject.GetComponent<TrajectoryProjection2DComponent>();
        for (int i = 0; i < removeOnCopy.Count; i++)
        {
            Destroy(trajectoryComponent.removeOnCopy[i]);
        }
        Destroy(trajectoryComponent);
    }
    #endregion
}
