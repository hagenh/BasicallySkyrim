using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private Attack Attack;
    private bool alreadyAppliedForce;

    public PlayerAttackState(PlayerStateMachine stateMachine, int AttackIndex) : base(stateMachine) 
    {
        Attack = stateMachine.Attacks[AttackIndex];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(Attack.AnimationName, Attack.TransitionDuration);
        Debug.Log("Attack: " + Attack.AnimationName);
    }

    public override void Tick()
    {
        ApplyGravity();
        Move();
        FaceTargetDirection();

        float normalizedTime = GetNormalizedAnimationTime();

        if(normalizedTime < 1f)
        {
            if (normalizedTime >= Attack.ForceTime) { TryApplyForce(); }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // go back to moveState
        }
    }

    public override void Exit()
    {
        
    }

    private void TryComboAttack(float normalizedTime)
    {
        if(Attack.ComboStateIndex == -1) { return; }

        if(normalizedTime < Attack.ComboAttackTime) { return; } 

        stateMachine.SwitchState(new PlayerAttackState(stateMachine, Attack.ComboStateIndex));
    }

    private float GetNormalizedAnimationTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        if(stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if(!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0;
        }
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; };

        Debug.Log("stateMachine and Attack: " + stateMachine.transform.forward + ", " + Attack.Force);
        stateMachine.ForceReciever.AddForce(stateMachine.transform.forward * Attack.Force);

        alreadyAppliedForce = true;
    }
}
