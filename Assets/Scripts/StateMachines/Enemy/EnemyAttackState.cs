using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    public delegate void PlayerIsAttacked();
    public static PlayerIsAttacked playerIsAttackedCallBack;

    Transform player;
    float attackRange = 1f;
    int timeBetweenAttacks = 3;
    float attackTimer;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        
    }

    public override void Tick()
    {
        ApplyGravity();

        // Rotate towards the player
        Vector3 directionToPlayer = stateMachine.player.transform.position - stateMachine.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation, Time.deltaTime * stateMachine.RotationSpeed);

        // Move towards the player
        if(Vector3.Distance(stateMachine.transform.position, stateMachine.player.transform.position) > attackRange)
        {
            stateMachine.transform.position += stateMachine.MovementSpeed * Time.deltaTime * stateMachine.transform.forward;
            return;
        }
        
        // Remove health from the player if attackTimer is >= timeBetweenAttacks
        if(attackTimer >= timeBetweenAttacks)
        {
            // Attack
            playerIsAttackedCallBack?.Invoke();
            attackTimer = 0;
        }
        else
        {
            attackTimer += Time.deltaTime;
        }


    }

    public override void Exit() { }
}
