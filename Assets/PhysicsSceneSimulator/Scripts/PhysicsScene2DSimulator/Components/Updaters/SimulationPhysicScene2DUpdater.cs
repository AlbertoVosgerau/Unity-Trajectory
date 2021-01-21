using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationPhysicScene2DUpdater : BaseSceneUpdater
{
    protected override void Awake()
    {
        base.Awake();
        _PhysicsScene = PhysicsScenes2D.simulationPhysicsScene;
    }
}
