using UnityEngine;

public struct TrajectoryProjectionPoint
{
    public Vector3 position;
    public Vector3 velocity;
    public float elapsedTime;

    public TrajectoryProjectionPoint(Vector3 position, Vector3 velocity, float elapsedTime)
    {
        this.position = position;
        this.velocity = velocity;
        this.elapsedTime = elapsedTime;
    }
}