using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShooterInputHandler : MonoBehaviour
{
    private TrajectoryProjectionComponent trajectory;
    private void Awake()
    {
        trajectory = GetComponent<TrajectoryProjectionComponent>();
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            trajectory.CancelSimulation();
        }
    }
}
