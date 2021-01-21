using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TrajectoryProjection2DComponent : MonoBehaviour
{
    #region Serialized Fields
    [Tooltip("Time in real time scene after which the simulation will end. If it is 0, time will be unlimited")]
    [SerializeField] private float simulationTimeLimit = 3f;
    [Tooltip("Layers that will fire finish event on collision for the simulations scene. Doesn't affect the real scene")]
    [SerializeField] private LayerMask simulationLayerMask = ~0;
    [Tooltip("Real action will not fire until any simulation has been started")]
    [SerializeField] private bool simulationRequired = false;
    [Tooltip("Real action will not fire until the simulation has finished")]
    [SerializeField] private bool waitSimulationFinish = false;
    [Tooltip("Real action will automatically fire on projection finish")]
    [SerializeField] private bool fireOnSimulationFinish = false;
    [Tooltip("Clear simulation when the projection simulation is finished")]
    [SerializeField] private bool clearOnSimulationFinish = false;
    [Tooltip("Hide real object renderer on simulation")]
    [SerializeField] private bool hideRendererOnSimulation = true;
    [Tooltip("When the real physics finish event happen, clear all simulations automatically")]
    [SerializeField] private bool clearOnRealPhysicsFinish = true;
    [Tooltip("Any component unecessary to the physics simulation can be added on this list. They will be removed from de simulation copy")]
    [SerializeField] private List<MonoBehaviour> removeOnCopy;
    #endregion

    #region Unity Events
    public UnityEvent<Rigidbody2D> onPhysicsAction;
    public UnityEvent<Transform, Transform> onVisualize;
    public UnityEvent onSimulationCanceled;
    public UnityEvent onSimulationFinished;
    public UnityEvent onRealPhysicsFinish;
    #endregion

    #region Private Variables
    private GameObject simulationContainer;
    private GameObject simObject;
    private TrajectoryProjection2DStatus status;
    private bool hasStartedASimulation = false;
    private bool isOnSimulation = false;
    private bool simulationFinished = false;
    private bool hasFired = false;
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
        if (simulationRequired && !hasStartedASimulation)
            return;

        if (simulationFinished && hasFired)
            return; 

        if (waitSimulationFinish && !simulationFinished)
            return;

        if(isOnSimulation)
        {
            CancelSimulation(false);
        }

        hasFired = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb == null)
            return;

        onPhysicsAction.Invoke(rb);
    }
    public void OnRealPhysicsFinish()
    {
        if (clearOnSimulationFinish)
            ClearProjection();
    }
    #endregion

    #region Simulation
    public void Simulate()
    {
        if (simulationFinished)
            return;

        if (isOnSimulation)
        {
            CancelSimulation();
            Simulate();
            return;
        }

        if (simulationContainer != null)
            ClearProjection();

        simulationFinished = false;
        simObject = SimulationObject();
        Rigidbody2D rb = simObject.GetComponent<Rigidbody2D>();
        onPhysicsAction.Invoke(rb);

        simulationContainer = new GameObject($"simulationContainer_{gameObject.name}");

        StartCoroutine(SimulationLoop());        
    }
    public void CancelSimulation(bool clearProjection = true)
    {
        if (!isOnSimulation)
            return;

        hasFired = false;
        isOnSimulation = false;
        status.onValidCollision -= OnSimulationFinished;
        Destroy(simObject);
        onSimulationCanceled.Invoke();
        if(clearProjection)
            ClearProjection();
    }
    public void ClearProjection()
    {
        Destroy(simulationContainer);
    }
    private void OnSimulationFinished()
    {
        hasStartedASimulation = true;

        if (!isOnSimulation)
            return;

        simulationFinished = true;
        isOnSimulation = false;
        status.onValidCollision -= OnSimulationFinished;
        Destroy(simObject);

        if (clearOnSimulationFinish)
            ClearProjection();

        onSimulationFinished.Invoke();

        if (fireOnSimulationFinish)
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

        if (isOnSimulation)
            OnSimulationFinished();
        else
            CancelSimulation();
    }
    #endregion

    #region Simulation GameObject
    private GameObject SimulationObject()
    {
        GameObject simObject = Instantiate(gameObject, transform.position, transform.rotation);
        simObject.name = $"virtual_{gameObject.name}";
        status = simObject.AddComponent<TrajectoryProjection2DStatus>();
        status.layerMask = simulationLayerMask;
        status.onValidCollision += OnSimulationFinished;
        PhysicsSceneObjectId id = simObject.GetComponent<PhysicsSceneObjectId>();
        id.SetIsOriginal(false);


        ClearComponents(simObject);

        MoveObjectToSimulationScene(simObject);
        return simObject;
    }
    private void MoveObjectToSimulationScene(GameObject objectToMove)
    {
        SceneManager.MoveGameObjectToScene(objectToMove, PhysicsScenes2D.simulationScene);
    }
    private void ClearComponents(GameObject simObject)
    {
        TrajectoryProjection2DComponent trajectoryComponent = simObject.GetComponent<TrajectoryProjection2DComponent>();
        for (int i = 0; i < removeOnCopy.Count; i++)
        {
            if(trajectoryComponent.removeOnCopy[i] != null)
                Destroy(trajectoryComponent.removeOnCopy[i]);
        }
        Destroy(trajectoryComponent);

        if (hideRendererOnSimulation)
        {
            Renderer renderer = simObject.GetComponent<Renderer>();
            Destroy(renderer);

            MeshFilter meshFilter = simObject.GetComponent<MeshFilter>();
            if (meshFilter != null)
                Destroy(meshFilter);
        }

        PhysicsScene2DCloneHandler cloneHandler = simObject.GetComponent<PhysicsScene2DCloneHandler>();
        if (cloneHandler != null)
            Destroy(cloneHandler);

        PhysicsScene2DSimpleTeleportComponent simpleTeleport = simObject.GetComponent<PhysicsScene2DSimpleTeleportComponent>();
        if (simpleTeleport != null)
            Destroy(simpleTeleport);

        PhysicsScene2DTeleportComponent teleport = simObject.GetComponent<PhysicsScene2DTeleportComponent>();
        if (teleport != null)
            Destroy(teleport);
    }
    #endregion

    #region RealPhysicsScene
    public void OnRealPhysicsFinish(Action action)
    {
        if(clearOnRealPhysicsFinish)
            ClearProjection();

        onRealPhysicsFinish.Invoke();
        action.Invoke();
    }
    #endregion
}
