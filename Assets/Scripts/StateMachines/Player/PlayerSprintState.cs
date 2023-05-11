using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    private readonly int SprintHash = Animator.StringToHash("Sprint");
    
    public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(SprintHash, 0.1f);
        
        stateMachine.InputReader.OnJumpPerformed += SwitchToJumpState;
        stateMachine.InputReader.OnInteractPerformed += CheckForInteractable;
        stateMachine.InputReader.OnTargetPerformed += SwitchToTargetingState;
        stateMachine.InputReader.OnCrouchPerformed += SwitchToCrouchState;
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

        /*var velocityNormalized = stateMachine.Velocity.normalized;
        if (velocityNormalized is { x: < 0.1f, y: < 0.1f })
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }*/
        
        CalculateMoveDirection(stateMachine.SprintSpeed);
        FaceMoveDirection();
        Move();
    }

    public override void Exit()
    {
        stateMachine.InputReader.OnJumpPerformed -= SwitchToJumpState;
        stateMachine.InputReader.OnInteractPerformed -= CheckForInteractable;
        stateMachine.InputReader.OnTargetPerformed -= SwitchToTargetingState;
        stateMachine.InputReader.OnCrouchPerformed += SwitchToCrouchState;
    }
}
