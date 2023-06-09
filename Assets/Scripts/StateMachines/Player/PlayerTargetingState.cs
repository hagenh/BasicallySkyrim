using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private const float CrossFadeDuration = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //stateMachine.Animator.Play(TargetingBlendTreeHash);
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.OnTargetCancelPerformed += SwitchToMoveState;
        stateMachine.InputReader.OnCrouchPerformed += SwitchToMoveState;
        stateMachine.InputReader.OnJumpPerformed += SwitchToJumpState;
    }

    public override void Tick()
    {
        if (stateMachine.InputReader.IsAttacking) 
        { 
            stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
            return;
        }
        if(stateMachine.Targeter.currentTarget == null)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }

        CalculateMoveDirection(stateMachine.MovementSpeed);
        FaceTargetDirection();
        Move();
    }

    public override void Exit()
    {
        stateMachine.InputReader.OnTargetCancelPerformed -= SwitchToMoveState;
        stateMachine.InputReader.OnCrouchPerformed -= SwitchToMoveState;
        stateMachine.InputReader.OnJumpPerformed -= SwitchToJumpState;
    }

    private void SwitchToMoveState()
    {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerMoveState(stateMachine));
    }
}
