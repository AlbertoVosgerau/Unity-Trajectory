using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneUpdater : MonoBehaviour
{
    #region Public Variables
    public TimeScaleType timeScaleType;
    public int timeIterations = 1;
    #endregion

    #region Protected Variables
    [SerializeField] protected bool enablePhysicsOnDestroy = true;
    protected PhysicsScene2D physicsScene;
    #endregion

    #region MonoBehaviour
    protected virtual void Awake()
    {
        RegisterOrCreateDefaultSceneUpdater();
        RegisgerOrCreateSceneHandler();
        Physics2D.simulationMode = SimulationMode2D.Script;
    }
    protected virtual void OnDestroy()
    {
        if (!enablePhysicsOnDestroy)
            return;

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    protected virtual void FixedUpdate()
    {
        if (!physicsScene.IsValid())
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
    #endregion

    #region Scene Time Updates
    protected virtual void SpeedUp()
    {
        for (int i = 0; i < timeIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    protected virtual void SlowDown()
    {
        physicsScene.Simulate(Time.fixedDeltaTime / timeIterations);
    }
    #endregion

    #region Scene Registration
    protected virtual void RegisterOrCreateDefaultSceneUpdater()
    {
        PhysicScene2DUpdater updater = FindObjectOfType<PhysicScene2DUpdater>();
        if (updater != null)
            return;
        GameObject newUpdater = new GameObject("PhysicsScene2DUpdater");
        newUpdater.AddComponent<PhysicScene2DUpdater>();
    }
    protected virtual void RegisgerOrCreateSceneHandler()
    {
        SceneRegisterHandler registerHandler = FindObjectOfType<SceneRegisterHandler>();
        if (registerHandler != null)
            return;
        GameObject newSceneRegisterHandler = new GameObject("SceneRegisterHandler");
        newSceneRegisterHandler.AddComponent<SceneRegisterHandler>();
    }
    #endregion
}
