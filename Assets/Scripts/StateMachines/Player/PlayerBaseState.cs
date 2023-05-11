using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine stateMachine;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void CalculateMoveDirection(float moveSpeed)
    {
        var forward = stateMachine.MainCamera.forward;
        Vector3 cameraForward = new(forward.x, 0, forward.z);
        var right = stateMachine.MainCamera.right;
        Vector3 cameraRight = new(right.x, 0, right.z);

        var moveDirection = cameraForward.normalized * stateMachine.InputReader.MoveComposite.y + cameraRight.normalized * stateMachine.InputReader.MoveComposite.x;

        stateMachine.Velocity.x = moveDirection.x * moveSpeed;
        stateMachine.Velocity.z = moveDirection.z * moveSpeed;
    }

    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(stateMachine.Velocity.x, 0f, stateMachine.Velocity.z);

        if (faceDirection == Vector3.zero)
            return;

        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.LookRotationDampFactor * Time.deltaTime);
    }

    protected void ApplyGravity()
    {
        if(stateMachine.Velocity.y > Physics.gravity.y)
        {
            stateMachine.Velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    protected void Move()
    {
        stateMachine.Controller.Move((stateMachine.Velocity + stateMachine.ForceReciever.Forces) * Time.deltaTime);
    }

    protected void CheckForInteractable()
    {
        if (Camera.main == null) return;
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (!Physics.Raycast(ray, out var hit, 100)) return;
        if (hit.collider.TryGetComponent<Interactable>(out var interactable))
        {
            SetFocus(interactable);
        }
        else
        {
            RemoveFocus();
        }
    }

    private void SetFocus(Interactable newFocus)
    {
        if (newFocus != stateMachine.focus)
        {
            if (stateMachine.focus != null)
            {
                stateMachine.focus.OnDefocused();
            }

            stateMachine.focus = newFocus;
        }

        newFocus?.OnFocused(stateMachine.transform);
    }

    private void RemoveFocus()
    {
        if (stateMachine.focus != null)
        {
            stateMachine.focus.OnDefocused();
        }

        stateMachine.focus = null;
    }

    protected void FaceTargetDirection()
    {
        if(stateMachine.Targeter.currentTarget == null) { return; }

        Vector3 faceDirection = stateMachine.Targeter.currentTarget.transform.position - stateMachine.transform.position;
        faceDirection.y = 0f;

        if (faceDirection == Vector3.zero) {  return; }

        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.LookRotationDampFactor * Time.deltaTime);
    }

    public void SwitchToCrouchState()
    {
        stateMachine.SwitchState(new PlayerCrouchState(stateMachine));
    }

    public void SwitchToJumpState()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }

    public void SwitchToInteractState()
    {
        stateMachine.SwitchState(new PlayerInteractState(stateMachine));
    }

    public void SwitchToTargetingState()
    {
        if (stateMachine.Targeter.SelectTarget())
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }

    public void SwitchToSprintState()
    {
        stateMachine.SwitchState(new PlayerSprintState(stateMachine));
    }
}
