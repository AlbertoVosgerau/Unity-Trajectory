using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrajectoryPredictionVisualizer : MonoBehaviour
{
    protected GameObject trajectoryContainer;
    
    public virtual void Start()
    {
        trajectoryContainer = new GameObject($"{gameObject.name}_TrajectoryContainer");
    }
    public virtual void Visualize(TrajectoryProjectionPoint trajectoryProjectionStep)
    {

    }
}
