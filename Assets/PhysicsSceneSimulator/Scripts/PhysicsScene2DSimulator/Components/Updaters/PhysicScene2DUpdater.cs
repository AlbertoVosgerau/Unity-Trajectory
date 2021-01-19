using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicScene2DUpdater : MonoBehaviour
{
    [SerializeField] private bool enablePhysicsOnDestroy = true;
    private void Awake()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;        
    }
    private void OnDestroy()
    {
        if (!enablePhysicsOnDestroy)
            return;

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    private void FixedUpdate()
    {
        if (!PhysicsScenes2D.currenScenePhysics.IsValid())
            return;

        PhysicsScenes2D.currenScenePhysics.Simulate(Time.fixedDeltaTime);
    }
}
