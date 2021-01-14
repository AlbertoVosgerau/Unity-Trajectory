using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ObjectShooter2DInputHandler))]
public class ObjectShooter2D : MonoBehaviour
{
    [SerializeField] private Vector2 force = new Vector2(5,5);

    public UnityEvent onDestroy;
    public void Shoot(Rigidbody2D rb)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onDestroy.Invoke();
        Destroy(gameObject);
    }
}
