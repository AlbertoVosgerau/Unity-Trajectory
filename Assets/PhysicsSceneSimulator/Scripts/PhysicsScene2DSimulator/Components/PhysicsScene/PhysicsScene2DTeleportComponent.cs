using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DTeleportComponent : MonoBehaviour
{
    #region Public Variables
    public bool ignoreNextExit = true;
    #endregion

    #region Private Variables
    private PhysicsScene2DCloneHandler cloneHandler;
    private Rigidbody2D rb;
    private Vector2 velocity;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cloneHandler = GetComponent<PhysicsScene2DCloneHandler>();
    }
    #endregion

    #region Swap Scenes
    public void SwapScenes(Scene from, Scene to)
    {
        if (cloneHandler == null)
            return;

        cloneHandler.SwapScene(from, to);
    }
    #endregion

    #region Rigidbody
    public void StoreRigidbodyData()
    {
        velocity = rb.velocity;
    }
    public void RestoreRigidbodyData()
    {
        rb.velocity = velocity;
    }
    #endregion
}
