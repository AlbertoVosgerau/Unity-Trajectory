using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicSceneUpdater : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
    }

    private void FixedUpdate()
    {
        if (!PhysicsScenes.currenScenePhysics.IsValid())
            return;

        PhysicsScenes.currenScenePhysics.Simulate(Time.fixedDeltaTime);
    }
}
