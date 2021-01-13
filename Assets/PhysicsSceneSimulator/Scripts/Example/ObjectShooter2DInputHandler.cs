using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShooter2DInputHandler : MonoBehaviour
{
    private TrajectoryProjection2DComponent trajectory;
    private void Awake()
    {
        trajectory = GetComponent<TrajectoryProjection2DComponent>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            trajectory.Simulate();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            trajectory.FireAction();
        }
    }
}
