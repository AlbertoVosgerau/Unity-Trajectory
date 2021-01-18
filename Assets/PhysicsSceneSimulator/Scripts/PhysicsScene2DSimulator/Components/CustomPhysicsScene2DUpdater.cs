using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPhysicsScene2DUpdater : MonoBehaviour
{
    public string sceneName;
    public TimeScaleType timeScaleType;
    public int timeIterations = 1;
    [SerializeField] private bool enablePhysicsOnDestroy = true;
    private int index;

    private void Awake()
    {
        RegisterOrCreateDefaultSceneUpdater();
        Physics2D.simulationMode = SimulationMode2D.Script;
        index = PhysicsScenes2D.RegisterNewScene2D(sceneName);
    }
    private void OnDestroy()
    {
        //PhysicsScenes2D.UnregisterScene2D(index);
        if (!enablePhysicsOnDestroy)
            return;

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    private void FixedUpdate()
    {
        if (!PhysicsScenes2D.customScenes[index].physicsScene.IsValid())
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
            PhysicsScenes2D.customScenes[index].physicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    private void SlowDown()
    {
        PhysicsScenes2D.customScenes[index].physicsScene.Simulate(Time.fixedDeltaTime / timeIterations);
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
