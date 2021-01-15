using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(PhysicScene2DUpdater))]
public class SimulationPhysicScene2DUpdater : MonoBehaviour
{
    public TimeScaleType timeScaleType;
    public int timeIterations = 1;
    [SerializeField] private bool enablePhysicsOnDestroy = true;
    private void Awake()
    {
        RegisterOrCreateDefaultSceneUpdater();
        Physics2D.simulationMode = SimulationMode2D.Script;
        PhysicsScenes2D.InitializePhysicsScene2D(SceneManager.GetActiveScene().name);
    }
    private void OnDestroy()
    {
        SceneManager.UnloadSceneAsync(PhysicsScenes2D.simulationScene);

        if (!enablePhysicsOnDestroy)
            return;

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    private void FixedUpdate()
    {
        if (!PhysicsScenes2D.simulationPhysicsScene.IsValid())
            return;

        switch (timeScaleType)
        {
            case TimeScaleType.SpeedUp:
                SpeedUp();
                break;
            case TimeScaleType.SlowDown:
                SlowDown();
                break;
            default:
                SpeedUp();
                break;
        }
    }

    private void SpeedUp()
    {
        for (int i = 0; i < timeIterations; i++)
        {
            PhysicsScenes2D.simulationPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    private void SlowDown()
    {
        PhysicsScenes2D.simulationPhysicsScene.Simulate(Time.fixedDeltaTime/timeIterations);
    }
    private void RegisterOrCreateDefaultSceneUpdater()
    {
        PhysicScene2DUpdater updater = FindObjectOfType<PhysicScene2DUpdater>();
        if (updater != null)
            return;
        GameObject newUpdater = new GameObject("PhysicsScene2DUpdater");
        newUpdater.AddComponent<PhysicScene2DUpdater>();
    }
}
