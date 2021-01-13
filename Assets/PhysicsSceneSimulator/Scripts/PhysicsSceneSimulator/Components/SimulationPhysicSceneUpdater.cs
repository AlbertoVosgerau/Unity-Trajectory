using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationPhysicSceneUpdater : MonoBehaviour
{
    [SerializeField] private int timeIterations = 1;
    [SerializeField] private bool enablePhysicsOnDestroy = true;
    private void Awake()
    {
        Physics.autoSimulation = false;
        PhysicsScenes.InitializePhysicsScene(SceneManager.GetActiveScene().name);
    }
    private void OnDestroy()
    {
        if (!enablePhysicsOnDestroy)
            return;

        Physics.autoSimulation = true;
    }
    private void FixedUpdate()
    {
        if (!PhysicsScenes.simulationPhysicsScene.IsValid())
            return;

        for (int i = 0; i < timeIterations; i++)
        {
            PhysicsScenes.simulationPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }
}
