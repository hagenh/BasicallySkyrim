using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{

    private readonly int CrouchWalkHash = Animator.StringToHash("CrouchWalk");

    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(CrouchWalkHash, 0.1f);
    }

    public override void Tick()
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
            return;
        }

        if (!stateMachine.Controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        CalculateMoveDirection(stateMachine.CrouchSpeed);
        FaceMoveDirection();
        Move();
    }

    public override void Exit() { }
}
