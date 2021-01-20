using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DTeleportComponent : MonoBehaviour
{
    [HideInInspector]
    public bool ignoreNextExit = true;
    private PhysicsScene2DCloneHandler cloneHandler;
    private Rigidbody2D rb;
    private Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cloneHandler = GetComponent<PhysicsScene2DCloneHandler>();
    }
    public void SwapScenes(Scene from, Scene to)
    {
        if (cloneHandler == null)
            return;

        cloneHandler.SwapScene(from, to);
    }
    public void StoreRigidbodyData()
    {
        velocity = rb.velocity;
    }
    public void RestoreRigidbodyData()
    {
        rb.velocity = velocity;
    }
}
