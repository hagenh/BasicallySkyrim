using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
    private const float AnimationDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Velocity.y = Physics.gravity.y;

        //stateMachine.Animator.Play(MoveBlendTreeHash);
        stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.OnJumpPerformed += SwitchToJumpState;
        stateMachine.InputReader.OnInteractPerformed += CheckForInteractable;
        stateMachine.InputReader.OnTargetPerformed += SwitchToTargetingState;
        stateMachine.InputReader.OnCrouchPerformed += SwitchToCrouchState;
        stateMachine.InputReader.OnCrouchPerformed += SwitchToCrouchState;
        stateMachine.InputReader.OnSprintPerformed += SwitchToSprintState;
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

        CalculateMoveDirection(stateMachine.MovementSpeed);
        FaceMoveDirection();
        Move();

        stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.MoveComposite.sqrMagnitude > 0 ? 1f : 0, AnimationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.OnJumpPerformed -= SwitchToJumpState;
        stateMachine.InputReader.OnInteractPerformed -= CheckForInteractable;
        stateMachine.InputReader.OnTargetPerformed -= SwitchToTargetingState;
        stateMachine.InputReader.OnCrouchPerformed += SwitchToCrouchState;
    }

}
