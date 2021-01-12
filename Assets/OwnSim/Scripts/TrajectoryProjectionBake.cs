using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryProjectionBake : MonoBehaviour
{
    public List<TrajectoryProjectionPoint> TrajectorySteps => trajectorySteps;

    private List<TrajectoryProjectionPoint> trajectorySteps = new List<TrajectoryProjectionPoint>();

    public TrajectoryProjectionPoint AddSectionToList(Vector3 position, Vector3 velocity, float elapsedTime)
    {
        TrajectoryProjectionPoint step = new TrajectoryProjectionPoint(position, velocity, elapsedTime);
        trajectorySteps.Add(step);
        return step;
    }

    public void RemoveStepFromList(int index)
    {
        trajectorySteps.RemoveAt(index);
    }

    public void ClearTrajectory()
    {
        trajectorySteps.Clear();
    }
}
