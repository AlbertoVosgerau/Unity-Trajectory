using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsScene2DSwitcher : MonoBehaviour
{
    public string sceneName;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private Collider2D originalCollider;


    bool isSomething;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isSomething)
                SwitchToOne();
            else
                SwitchToTwo();
            isSomething = !isSomething;               
        }
    }

    public void MoveObjectToCurrentScene()
    {
        if (string.IsNullOrEmpty(sceneName))
            return;

        SceneManager.MoveGameObjectToScene(gameObject, PhysicsScenes2D.currentScene);
    }

    public void MoveObjectToCustomScene2D()
    {
        if (string.IsNullOrEmpty(sceneName))
            return;

        StoreRigidbodyData();
        int index = PhysicsScenes2D.CustomScene2DIndex(sceneName);
        SceneManager.MoveGameObjectToScene(gameObject, PhysicsScenes2D.customScenes[index].scene);
        RestoreRigidbodyData();
    }

    private void SwitchToOne()
    {
        StoreRigidbodyData();
        int index = PhysicsScenes2D.CustomScene2DIndex("CustomScene");
        SceneManager.MoveGameObjectToScene(gameObject, PhysicsScenes2D.customScenes[index].scene);
        RestoreRigidbodyData();
    }

    private void SwitchToTwo()
    {
        StoreRigidbodyData();
        int index = PhysicsScenes2D.CustomScene2DIndex("CustomScene2");
        SceneManager.MoveGameObjectToScene(gameObject, PhysicsScenes2D.customScenes[index].scene);
        RestoreRigidbodyData();
    }

    private void StoreRigidbodyData()
    {
        velocity = rb.velocity;
    }
    private void RestoreRigidbodyData()
    {
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (originalCollider != null)
            return;
        originalCollider = collision;

        Debug.Log("Trigger Enter");
        SwitchToOne();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == originalCollider)
            return;

        Debug.Log("Trigger Exit");
        SwitchToTwo();
    }
}
