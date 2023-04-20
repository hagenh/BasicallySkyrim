using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public Transform[] wayPoints;
    private int currentWayPointIndex = 0;
    private float shouldWaitProbability = 6;

    public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        wayPoints = new Transform[10] {
        GameObject.Find("WayPoint1").transform,
        GameObject.Find("WayPoint2").transform,
        GameObject.Find("WayPoint3").transform,
        GameObject.Find("WayPoint4").transform,
        GameObject.Find("WayPoint5").transform,
        GameObject.Find("WayPoint6").transform,
        GameObject.Find("WayPoint7").transform,
        GameObject.Find("WayPoint8").transform,
        GameObject.Find("WayPoint9").transform,
        GameObject.Find("WayPoint10").transform
        };

        currentWayPointIndex = GetClosestEnemy(wayPoints);
    }

    public override void Tick()
    {
        ApplyGravity();
        
        Vector3 directionToPlayer = stateMachine.player.transform.position + Vector3.up - stateMachine.transform.position;
        float angleToPlayer = Vector3.Angle(stateMachine.transform.forward, directionToPlayer);

        if (angleToPlayer < stateMachine.fieldOfView / 2f)
        {
            if (Physics.Raycast(stateMachine.transform.position, directionToPlayer, out RaycastHit hit, stateMachine.viewDistance, stateMachine.obstacleMask))
            {
                if (hit.transform == stateMachine.player.transform)
                {
                    stateMachine.SwitchState(new EnemyAttackState(stateMachine));
                }
            }
        }

        Transform targetWayPoint = wayPoints[currentWayPointIndex];

        if (Vector3.Distance(stateMachine.transform.position, targetWayPoint.position) < 0.01f ) {
            
            //See if an enemy should wait during his patrol
            var rangeFloat = Random.Range(-10.0f, 10.0f);
            if(rangeFloat > shouldWaitProbability)
            {
                stateMachine.SwitchState( new EnemyPatrolWaitState(stateMachine));
            }

            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
            return;
        }

        Vector3 faceDirection = targetWayPoint.position;
        FaceTowardsWayPoint(faceDirection);

        stateMachine.transform.position = Vector3.MoveTowards(stateMachine.transform.position, targetWayPoint.position, stateMachine.MovementSpeed * Time.deltaTime);

    }

    public override void Exit() { }
}
