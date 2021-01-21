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
    #endregion

    #region Private Variables
    private GameObject _SimulationContainer;
    private GameObject _SimObject;
    private TrajectoryProjection2DStatus _Status;
    private bool _HasStartedASimulation = false;
    private bool _IsOnSimulation = false;
    private bool _SimulationFinished = false;
    private bool _HasFired = false;
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
        if (simulationRequired && !_HasStartedASimulation)
            return;

        if (_SimulationFinished && _HasFired)
            return; 

        if (waitSimulationFinish && !_SimulationFinished)
            return;

        if(_IsOnSimulation)
        {
            CancelSimulation(false);
        }

        _HasFired = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb == null)
            return;

        onPhysicsAction.Invoke(rb);
    }
    public void OnRealPhysicsFinish()
    {
        if (clearOnRealPhysicsFinish)
            ClearProjection();
    }
    #endregion

    #region Simulation
    public void Simulate()
    {
        if (_SimulationFinished)
            return;

        if (_IsOnSimulation)
        {
            CancelSimulation();
            Simulate();
            return;
        }

        if (_SimulationContainer != null)
            ClearProjection();

        _SimulationFinished = false;
        _SimObject = SimulationObject();
        Rigidbody2D rb = _SimObject.GetComponent<Rigidbody2D>();
        onPhysicsAction.Invoke(rb);

        _SimulationContainer = new GameObject($"simulationContainer_{gameObject.name}");

        StartCoroutine(SimulationLoop());        
    }
    public void CancelSimulation(bool clearProjection = true)
    {
        if (!_IsOnSimulation)
            return;

        _HasFired = false;
        _IsOnSimulation = false;
        _Status.onValidCollision -= OnSimulationFinished;
        Destroy(_SimObject);
        onSimulationCanceled.Invoke();
        if(clearProjection)
            ClearProjection();
    }
    public void ClearProjection()
    {
        Destroy(_SimulationContainer);
    }
    private void OnSimulationFinished()
    {
        _HasStartedASimulation = true;

        if (!_IsOnSimulation)
            return;

        _SimulationFinished = true;
        _IsOnSimulation = false;
        _Status.onValidCollision -= OnSimulationFinished;
        Destroy(_SimObject);

        if (clearOnSimulationFinish)
            ClearProjection();

        onSimulationFinished.Invoke();

        if (fireOnSimulationFinish)
            FireAction();
    }
    private IEnumerator SimulationLoop()
    {
        int count = 0;
        _IsOnSimulation = true;
        if(simulationTimeLimit != 0)
            StartCoroutine(SimulationClock());
        while (_IsOnSimulation)
        {
            if (_SimulationContainer == null)
                yield break;

            onVisualize.Invoke(_SimObject.transform, _SimulationContainer.transform);
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

        if (_IsOnSimulation)
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
        _Status = simObject.AddComponent<TrajectoryProjection2DStatus>();
        _Status.layerMask = simulationLayerMask;
        _Status.onValidCollision += OnSimulationFinished;
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

        PhysicsScene2DTeleportAreaComponent teleport = simObject.GetComponent<PhysicsScene2DTeleportAreaComponent>();
        if (teleport != null)
            Destroy(teleport);
    }
    #endregion
}
