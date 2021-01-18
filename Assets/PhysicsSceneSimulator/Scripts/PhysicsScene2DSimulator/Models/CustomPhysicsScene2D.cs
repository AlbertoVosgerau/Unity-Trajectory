using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPhysicsScene2D
{
    public string sceneName;
    [HideInInspector]
    public Scene scene;
    public PhysicsScene2D physicsScene;
    public CustomPhysicsScene2D(string sceneName)
    {
        this.sceneName = sceneName;

        CreateSceneParameters sceneParams = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        scene = SceneManager.CreateScene(sceneName, sceneParams);
        physicsScene = scene.GetPhysicsScene2D();
        scene.name = sceneName;
    }
}
