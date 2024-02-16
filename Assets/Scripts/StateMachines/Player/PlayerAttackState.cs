using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private Attack Attack;

    private bool alreadyAppliedForce;
    private float previousFrameTime;

    public PlayerAttackState(PlayerStateMachine stateMachine, int AttackIndex) : base(stateMachine) 
    {
        Attack = stateMachine.Attacks[AttackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(Attack.Damage);
        stateMachine.Animator.CrossFadeInFixedTime(Attack.AnimationName, Attack.TransitionDuration);
    }

    public override void Tick()
    {
        ApplyGravity();
        CalculateMoveDirection(stateMachine.MovementSpeed);
        FaceTargetDirection();
        stateMachine.Controller.Move((stateMachine.Velocity / 4 + stateMachine.ForceReciever.Forces) * Time.deltaTime);

        float normalizedTime = GetNormalizedAnimationTime();

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= Attack.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.currentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;

    }

    public override void Exit()
    {
        
    }

    private void TryComboAttack(float normalizedTime)
    {
        if(Attack.ComboStateIndex == -1) { return; }

        if(normalizedTime < Attack.ComboAttackTime) {  return; }

        stateMachine.SwitchState(new PlayerAttackState(stateMachine, Attack.ComboStateIndex));
    }

    private float GetNormalizedAnimationTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }

    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; };

        stateMachine.ForceReciever.AddForce(stateMachine, stateMachine.transform.forward * Attack.Force);

        alreadyAppliedForce = true;
    }
}
