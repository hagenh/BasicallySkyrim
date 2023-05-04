using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    public delegate void PlayerIsAttacked();
    public static PlayerIsAttacked playerIsAttackedCallBack;

    private Attack Attack;
    private bool hasPlayedAnimation = false;

    public EnemyAttackState(EnemyStateMachine stateMachine, int AttackIndex) : base(stateMachine) 
    {
        Attack = stateMachine.Attacks[AttackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(Attack.Damage);
    }

    public override void Tick()
    {
        if (CheckForDeath()) { stateMachine.SwitchState(new EnemyDeathState(stateMachine)); return; }
        ApplyGravity();

        Debug.Log("Distance: " + Vector3.Distance(stateMachine.transform.position, stateMachine.player.transform.position));
        if (Vector3.Distance(stateMachine.transform.position, stateMachine.player.transform.position) <= Attack.AttackRange)
        {
            if (!hasPlayedAnimation)
            {
                Debug.Log("Attempting to run Attack animation");
                stateMachine.Animator.CrossFadeInFixedTime(Attack.AnimationName, Attack.TransitionDuration);
                hasPlayedAnimation = true;
            }
        }
        else
        {
            Vector3 directionToTarget = stateMachine.player.transform.position - stateMachine.navMeshAgent.transform.position;
            Vector3 normalizedDirection = directionToTarget.normalized;

            Vector3 finalPosition = stateMachine.player.transform.position + (normalizedDirection * Attack.AttackRange);
            stateMachine.navMeshAgent.SetDestination(finalPosition);
        }

        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(Attack.AnimationName) && stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (Vector3.Distance(stateMachine.transform.position, stateMachine.player.transform.position) < 2f)
            {
                Vector3 pos = stateMachine.transform.position + (-stateMachine.transform.forward * 2);
                stateMachine.navMeshAgent.SetDestination(pos);
                Debug.Log("Moved backwards enough!");
                return;
            }

            stateMachine.SwitchState(new EnemyChaseState(stateMachine));
        }
    }

    public override void Exit() { }
}
