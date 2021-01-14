using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPhysicsScene2DUpdater : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private TimeScaleType timeScaleType;
    [SerializeField] private int timeIterations = 1;
    [SerializeField] private bool enablePhysicsOnDestroy = true;
    private int index;

    private void Awake()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        index = PhysicsScenes2D.RegisterNewScene2D(sceneName);
    }
    private void OnDestroy()
    {
        SceneManager.UnloadSceneAsync(PhysicsScenes2D.simulationScene);
        PhysicsScenes2D.UnregisterScene2D(index);
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
}
