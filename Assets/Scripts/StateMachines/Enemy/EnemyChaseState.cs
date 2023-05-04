using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    readonly float chaseRange = 20f;
    readonly float attackRange = 3f;
    readonly float stoppingDistance = 2.0f;
    Vector3 circlePoint;

    private readonly int timeBetweenAttacks = 3;
    private float attackTimer;

    private readonly int WalkHash = Animator.StringToHash("Walk");

    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(WalkHash, 0.1f);
    }

    public override void Tick()
    {
        if (CheckForDeath()) { stateMachine.SwitchState(new EnemyDeathState(stateMachine)); return; }
        ApplyGravity();

        // Rotate towards the player
        Vector3 directionToPlayer = stateMachine.player.transform.position - stateMachine.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation, Time.deltaTime * stateMachine.RotationSpeed);

        // Check if inside of chase range
        if (Vector3.Distance(stateMachine.transform.position, stateMachine.player.transform.position) > chaseRange)
        {
            stateMachine.SwitchState(new EnemyPatrolState(stateMachine));
            return;
        }

        // Move towards the player
        if (Vector3.Distance(stateMachine.transform.position, stateMachine.player.transform.position) > attackRange)
        {
            Vector3 directionToTarget = stateMachine.player.transform.position - stateMachine.navMeshAgent.transform.position;
            Vector3 normalizedDirection = directionToTarget.normalized;

            Vector3 finalPosition = stateMachine.player.transform.position - normalizedDirection * stoppingDistance;
            stateMachine.navMeshAgent.SetDestination(finalPosition);
            
            //stateMachine.navMeshAgent.SetDestination(stateMachine.player.transform.position + new Vector3(3, 0, 0));
            return;
        }

        // Remove health from the player if attackTimer is >= timeBetweenAttacks
        if (attackTimer >= timeBetweenAttacks)
        {
            Debug.Log("Switching to AttackState!");
            stateMachine.SwitchState(new EnemyAttackState(stateMachine, 0));
        }
        else
        {
            attackTimer += Time.deltaTime;
            Debug.Log("AttackTimer: " + attackTimer);

            //var movePoint = stateMachine.transform.position + new Vector3(Random.Range(1,3), 0, 0);
            //stateMachine.navMeshAgent.SetDestination(movePoint);
        }

    }

    public override void Exit()
    {

    }
}
