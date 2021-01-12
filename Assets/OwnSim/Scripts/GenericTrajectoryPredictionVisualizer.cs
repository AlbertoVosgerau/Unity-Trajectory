using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTrajectoryPredictionVisualizer : BaseTrajectoryPredictionVisualizer
{
    public GameObject objectToInstantiate;
    public override void Visualize(TrajectoryProjectionPoint trajectoryProjectionStep)
    {
        Instantiate(objectToInstantiate, trajectoryProjectionStep.position, Quaternion.identity, trajectoryContainer.transform);
    }
}
