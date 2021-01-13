using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShooter : MonoBehaviour
{
    [SerializeField] private Vector3 force = new Vector3(5, 5, 0);
    public void Shoot(Rigidbody rb)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }
}
