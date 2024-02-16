using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIdleState : EnemyBaseState
{
    private float timeUntilAttackReady;
    private float timer;
    private readonly int fightIdleHash = Animator.StringToHash("FightIdle");

    public EnemyAttackIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(fightIdleHash, 0.1f);
        timeUntilAttackReady = Random.Range(2f, 4f);
    }

    public override void Tick()
    {
        if(CheckForDeath()) { stateMachine.SwitchState(new EnemyDeathState(stateMachine)); return; }

        ApplyGravity();

        if(timer >= timeUntilAttackReady)
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine, 0));
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    public override void Exit()
    {
        
    }
}
