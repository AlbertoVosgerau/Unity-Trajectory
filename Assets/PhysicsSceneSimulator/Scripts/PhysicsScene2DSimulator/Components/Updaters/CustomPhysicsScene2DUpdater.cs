using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPhysicsScene2DUpdater : BaseSceneUpdater
{
    public string SceneName => $"{_BaseSceneName} - {timeScaleType} - {timeIterations}x";
    public string _BaseSceneName = "Custom Scene";
    private int _Index;

    public void RegisterScene()
    {
        _Index = PhysicsScenes2D.RegisterNewScene2D(SceneName);
        _PhysicsScene = PhysicsScenes2D.customScenes[_Index].PhysicsScene;
    }
}
