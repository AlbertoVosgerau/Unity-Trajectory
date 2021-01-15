using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSceneUpdater : MonoBehaviour
{
    [SerializeField] private bool enablePhysicsOnDestroy = true;
    private void Awake()
    {
        Physics.autoSimulation = false;
        PhysicsScenes.InitializePhysicsScene(SceneManager.GetActiveScene().name);
    }
    private void OnDestroy()
    {
        if (!enablePhysicsOnDestroy)
            return;

        Physics.autoSimulation = true;
    }
    private void FixedUpdate()
    {
        if (!PhysicsScenes.currenScenePhysics.IsValid())
            return;

        PhysicsScenes.currenScenePhysics.Simulate(Time.fixedDeltaTime);
    }
}
