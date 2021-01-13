using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationPhysicScene2DUpdater : MonoBehaviour
{
    [SerializeField] private int timeIterations = 1;
    [SerializeField] private bool enablePhysicsOnDestroy = true;
    private void Awake()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        PhysicsScenes2D.InitializePhysicsScene2D(SceneManager.GetActiveScene().name);
    }
    private void OnDestroy()
    {
        if (!enablePhysicsOnDestroy)
            return;

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    private void FixedUpdate()
    {
        if (!PhysicsScenes2D.simulationPhysicsScene.IsValid())
            return;

        for (int i = 0; i < timeIterations; i++)
        {
            PhysicsScenes2D.simulationPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }
}
