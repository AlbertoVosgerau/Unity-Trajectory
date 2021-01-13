using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryProjectionComponent : MonoBehaviour
{
    public int timeIterations = 1;
    public Action<GameObject> onApplyPhysicsAction;

    private TrajectoryProjectionBake trajectoryBake;
    private BaseTrajectoryPredictionVisualizer predictionVisualizer;
    private BaseProjectileAction projectileAction;

    private void Awake()
    {        
        ResolveComponents();
    }
    private void Start()
    {
        InitializeScenes();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Simulate();
        }
    }
    public void Simulate()
    {
        GameObject simObject = Instantiate(gameObject, transform.position, transform.rotation);
        simObject.name = $"virtual_{gameObject.name}";
        Rigidbody2D rb = simObject.GetComponent<Rigidbody2D>();
        BaseTrajectoryPredictionStatus simulationStatus = simObject.GetComponent<BaseTrajectoryPredictionStatus>();


        MoveObjectToSimulationScene(simObject);
        onApplyPhysicsAction.Invoke(simObject);
        
        StartCoroutine(SimulationLoop(simObject, rb, simulationStatus));
        
    }
    private void ResolveComponents()
    {
        trajectoryBake = GetComponent<TrajectoryProjectionBake>();
        predictionVisualizer = GetComponent<BaseTrajectoryPredictionVisualizer>();
        projectileAction = GetComponent<BaseProjectileAction>();
    }
    private void InitializeScenes()
    {        
        PhysicsScenes.currentScene = SceneManager.GetActiveScene();
        PhysicsScenes.currenScenePhysics = PhysicsScenes.currentScene.GetPhysicsScene2D();
        PhysicsScenes.SetSimulationScene();
    }

    private IEnumerator SimulationLoop(GameObject simObject, Rigidbody2D rb, BaseTrajectoryPredictionStatus status)
    {
        int count = 0;
        while (count < 500)
        {
            for (int i = 0; i < timeIterations; i++)
            {
                PhysicsScenes.simulationPhysicsScene.Simulate(Time.fixedDeltaTime);
            }
            TrajectoryProjectionPoint predictionPoint = trajectoryBake.AddSectionToList(simObject.transform.position, rb.velocity, 0.2f);
            Debug.Log(simObject.transform.position);
            predictionVisualizer.Visualize(predictionPoint);
            count++;
            yield return null;
        }
        Destroy(simObject);
        projectileAction.ExecuteAction();
        yield return null;
    }

    private void MoveObjectToSimulationScene(GameObject objectToMove)
    {
        SceneManager.MoveGameObjectToScene(objectToMove, PhysicsScenes.simulationScene);
    }
}
