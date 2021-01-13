using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectShooter2DInputHandler))]
public class OjbectShooter2D : MonoBehaviour
{
    [SerializeField] private Vector2 force = new Vector2(5,5);
    public void Shoot(Rigidbody2D rb)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
