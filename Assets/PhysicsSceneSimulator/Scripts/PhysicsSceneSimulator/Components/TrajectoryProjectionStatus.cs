using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryProjectionStatus : MonoBehaviour
{
    public LayerMask layerMask;
    public Action onValidCollision;
    private void OnCollisionEnter(Collision collision)
    {
        if (!CheckLayerMask(layerMask, collision.gameObject.layer))
            return;

        onValidCollision.Invoke();
    }

    public bool CheckLayerMask(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
