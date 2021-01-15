using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DTeleportComponent : MonoBehaviour
{
    [HideInInspector]
    public bool isOnTriggerArea;
    [HideInInspector]
    public string storedId;
    private Rigidbody2D rb;
    private Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
