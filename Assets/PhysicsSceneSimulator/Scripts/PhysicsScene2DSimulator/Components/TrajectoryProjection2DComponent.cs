using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TrajectoryProjection2DComponent : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float simulationTimeLimit = 3f;
    [SerializeField] private LayerMask simulationLayerMask = ~0;
    [SerializeField] private bool fireAcionOnProjectionFinish = false;
    [SerializeField] private bool clearProjectionOnFinish = false;
    [SerializeField] private bool hideRendererOnSimulation = true;
    [SerializeField] private bool clearProjectionOnRealPhysicsFinish = true;
    [SerializeField] private List<MonoBehaviour> removeOnCopy;
    #endregion

    #region Unity Events
    public UnityEvent<Rigidbody2D> onPhysicsAction;
    public UnityEvent<Transform, Transform> onVisualize;
    public UnityEvent onSimulationFinished;
    public UnityEvent onRealPhysicsFinish;
    #endregion

    #region Private Variables
    private GameObject simulationContainer;
    private GameObject simObject;
    private TrajectoryProjection2DStatus status;
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
        status.onValidCollision -= OnSimulationFinished;
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
        status.onValidCollision -= OnSimulationFinished;
        Destroy(simObject);

        if (clearProjectionOnFinish)
            ClearProjection();

        onSimulationFinished.Invoke();

        if (fireAcionOnProjectionFinish)
            FireAction();
    }
    private IEnumerator SimulationLoop()
    {
        int count = 0;
        isOnSimulation = true;
        if(simulationTimeLimit != 0)
            StartCoroutine(SimulationClock());
        while (isOnSimulation)
        {
            onVisualize.Invoke(simObject.transform, simulationContainer.transform);
            count++;
            yield return null;
        }
        OnSimulationFinished();
    }
    private IEnumerator SimulationClock()
    {
        float time = 0;
        while(time < simulationTimeLimit)
        {
            time += Time.deltaTime;
            yield return null;
        }

        if(isOnSimulation)
            OnSimulationFinished();
    }
    #endregion

    #region Simulation GameObject
    private GameObject SimObject()
    {
        GameObject simObject = Instantiate(gameObject, transform.position, transform.rotation);
        simObject.name = $"virtual_{gameObject.name}";
        status = simObject.AddComponent<TrajectoryProjection2DStatus>();
        status.layerMask = simulationLayerMask;
        status.onValidCollision += OnSimulationFinished;
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

    #region RealPhysicsScene
    public void OnRealPhysicsFinish(Action action)
    {
        if(clearProjectionOnRealPhysicsFinish)
            ClearProjection();

        onRealPhysicsFinish.Invoke();
        action.Invoke();
    }
    #endregion
}
