using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNotifier : MonoBehaviour
{
    public bool hit;

    private void OnCollisionEnter(Collision collision)
    {
        hit = true;
    }
}
