using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    public bool isSecondScene = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        if(isSecondScene)
        {
            PhysicsScenes2D.UnregisterAll();
            SceneManager.LoadScene(1);
            return;
        }

        PhysicsScenes2D.UnregisterAll();
        SceneManager.LoadScene(0);
    }
}
