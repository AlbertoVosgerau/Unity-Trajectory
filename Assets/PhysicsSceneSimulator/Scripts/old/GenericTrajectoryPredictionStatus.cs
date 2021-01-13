using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTrajectoryPredictionStatus : BaseTrajectoryPredictionStatus
{
    private void OnCollisionEnter(Collision collision)
    {
        hasFinished = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStarted)
            return;
        hasFinished = true;
    }
}
