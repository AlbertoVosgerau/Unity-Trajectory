using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    public bool isSecondScene = false;
    public SceneRegisterHandler registerHandler;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Load1();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load2();
        }
    }

    private void Load1()
    {
        if (registerHandler == null)
            registerHandler = FindObjectOfType<SceneRegisterHandler>();

        registerHandler.LoadNewScene("PhysicsSceneSimulator2D");
    }
    private void Load2()
    {
        if (registerHandler == null)
            registerHandler = FindObjectOfType<SceneRegisterHandler>();

        registerHandler.LoadNewScene("PhysicsSceneSimulator2D-2");
    }
}
