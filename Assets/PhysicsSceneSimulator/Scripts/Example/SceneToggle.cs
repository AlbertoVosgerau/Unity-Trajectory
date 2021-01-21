using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    public bool isSecondScene = false;

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
        SceneLoadingRegisterHandler.Instance.LoadNewScene("PhysicsSceneSimulator2D");
    }
    private void Load2()
    {
        SceneLoadingRegisterHandler.Instance.LoadNewScene("PhysicsSceneSimulator2D-2");
    }
}
