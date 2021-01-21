using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPhysicsScene2DUpdater : BaseSceneUpdater
{
    public string SceneName => $"{baseSceneName} - {timeScaleType} - {timeIterations}x";
    public string baseSceneName = "Custom Scene";
    private int index;

    public void RegisterScene()
    {
        index = PhysicsScenes2D.RegisterNewScene2D(SceneName);
        physicsScene = PhysicsScenes2D.customScenes[index].physicsScene;
    }
}
