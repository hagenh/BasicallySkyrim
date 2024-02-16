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

        var moveInput = stateMachine.InputReader.MoveComposite.sqrMagnitude;

        float moveSpeed = 0;
        if(moveInput > 0 && moveInput < 1)
        {
            moveSpeed = 1f;
        }
        else if(moveInput >= 1)
        {
            moveSpeed = 2f;
        }
        else
        {
            moveSpeed = 0;
        }

        stateMachine.Animator.SetFloat(MoveSpeedHash, moveSpeed, AnimationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.OnJumpPerformed -= SwitchToJumpState;
        stateMachine.InputReader.OnInteractPerformed -= CheckForInteractable;
        stateMachine.InputReader.OnTargetPerformed -= SwitchToTargetingState;
    }

}
