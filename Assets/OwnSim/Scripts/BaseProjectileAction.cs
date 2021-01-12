using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileAction : MonoBehaviour
{
    public bool useBakedTrajectory = true;
    protected TrajectoryProjectionComponent trajectoryProjectionComponent;

    private void Awake()
    {
        trajectoryProjectionComponent = GetComponent<TrajectoryProjectionComponent>();
    }
    private void OnEnable()
    {
        trajectoryProjectionComponent.onApplyPhysicsAction = PhysicsAction;
    }
    public virtual void ExecuteAction(List<TrajectoryProjectionPoint> trajectoryProjectionSteps = null)
    {
        if ((useBakedTrajectory && trajectoryProjectionSteps != null) && trajectoryProjectionComponent != null)
            PlayBakedPrediction(trajectoryProjectionSteps);
        else
            PlayWithPhysics();
    }
    public virtual void PhysicsAction(GameObject actionObject)
    {
        
    }
    public virtual void PlayWithPhysics()
    {
        PhysicsAction(gameObject);
    }

    public virtual void PlayBakedPrediction(List<TrajectoryProjectionPoint> trajectoryProjectionSteps = null)
    {
        if (trajectoryProjectionSteps == null)
        {
            PlayWithPhysics();
            return;
        }

        for (int i = 0; i < trajectoryProjectionSteps.Count; i++)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    public void StopBakedExecution()
    {

    }

    private IEnumerator MoveToTarget()
    {
        yield return null;
    }
}
