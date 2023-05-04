using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public abstract class EnemyBaseState : State
{
    protected readonly EnemyStateMachine stateMachine;

    protected EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void FaceTowardsWayPoint(Vector3 faceDirection)
    {
        if (faceDirection == Vector3.zero)
            return;

        //find the vector pointing from our position to the target
        var lookDirection = (faceDirection - stateMachine.transform.position).normalized;

        //create the rotation we need to be in to look at the target
        var lookRotation = Quaternion.LookRotation(lookDirection);

        //rotate us over time according to speed until we are in the required rotation
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation, Time.deltaTime * stateMachine.RotationSpeed);
    }

    protected void ApplyGravity()
    {
        if (stateMachine.Velocity.y > Physics.gravity.y)
        {
            stateMachine.Velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    protected int GetClosestEnemy(Transform[] wayPoints)
    {
        int closestWayPointIndex = 0;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 enemyCurrentPosition = stateMachine.transform.position;

        for (int i = 0; i < wayPoints.Length; i++)
        {
            Vector3 directionToTarget = wayPoints[i].position - enemyCurrentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestWayPointIndex = i;
            }
        }

        return closestWayPointIndex;
    }

    public bool CheckForDeath()
    {
        if (stateMachine.Vitals.health <= 0) { return true; }
        return false;
    }
}