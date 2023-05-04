using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolWaitState : EnemyBaseState
{
    private float howLongToWait;
    private float waitTimer = 0f;

    private readonly int IdleHash = Animator.StringToHash("Idle_LookingDown");

    public EnemyPatrolWaitState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, 0.1f);
        howLongToWait = Random.Range(1f, 10.0f);
    }

    public override void Tick()
    {
        if (waitTimer > howLongToWait)
        {
            stateMachine.SwitchState(new EnemyPatrolState(stateMachine));
        }

        // TODO: Add Wait animation
        waitTimer += Time.deltaTime;
    }

    public override void Exit() { }

    
}
