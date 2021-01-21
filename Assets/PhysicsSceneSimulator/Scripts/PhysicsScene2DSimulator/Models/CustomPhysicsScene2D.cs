using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPhysicsScene2D
{
    public string SceneName => _sceneName;
    private string _sceneName;
    public Scene CustomScene => _customScene;
    private Scene _customScene;
    public PhysicsScene2D PhysicsScene => _physicsScene;
    private PhysicsScene2D _physicsScene;
    public CustomPhysicsScene2D(string sceneName)
    {
        _sceneName = sceneName;

        CreateSceneParameters sceneParams = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        _customScene = SceneManager.CreateScene(sceneName, sceneParams);
        _physicsScene = CustomScene.GetPhysicsScene2D();
        _customScene.name = sceneName;
    }
}
