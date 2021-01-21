using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DTeleportAreaComponent : MonoBehaviour
{
    #region Public Variables
    public bool ignoreNextExit = true;
    #endregion

    #region Private Variables
    private PhysicsScene2DCloneHandler _CloneHandler;
    private Rigidbody2D _Rb;
    private Vector2 _Velocity;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        _Rb = GetComponent<Rigidbody2D>();
        _CloneHandler = GetComponent<PhysicsScene2DCloneHandler>();
    }
    #endregion

    #region Swap Scenes
    public void SwapScenes(Scene from, Scene to)
    {
        if (_CloneHandler == null)
            return;

        _CloneHandler.SwapScene(from, to);
    }
    #endregion

    #region Rigidbody
    public void StoreRigidbodyData()
    {
        _Velocity = _Rb.velocity;
    }
    public void RestoreRigidbodyData()
    {
        _Rb.velocity = _Velocity;
    }
    #endregion
}
