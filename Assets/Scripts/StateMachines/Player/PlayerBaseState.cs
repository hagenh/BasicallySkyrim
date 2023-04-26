using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine stateMachine;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void CalculateMoveDirection()
    {
        Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * stateMachine.InputReader.MoveComposite.y + cameraRight.normalized * stateMachine.InputReader.MoveComposite.x;

        stateMachine.Velocity.x = moveDirection.x * stateMachine.MovementSpeed;
        stateMachine.Velocity.z = moveDirection.z * stateMachine.MovementSpeed;
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
        stateMachine.Controller.Move((stateMachine.Velocity /*+ stateMachine.ForceReciever.Forces*/) * Time.deltaTime);
    }

    public void CheckForInteractable()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            if (hit.collider.TryGetComponent<Interactable>(out var interactable))
            {
                SetFocus(interactable);
            }
            else
            {
                RemoveFocus();
            }
        }
    }

    void SetFocus(Interactable newFocus)
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

    void RemoveFocus()
    {
        if (stateMachine.focus != null)
        {
            stateMachine.focus.OnDefocused();
        }

        stateMachine.focus = null;
    }

    public void FaceTargetDirection()
    {
        if(stateMachine.Targeter.currentTarget == null) { return; }

        Vector3 faceDirection = stateMachine.Targeter.currentTarget.transform.position - stateMachine.transform.position;
        faceDirection.y = 0f;

        if (faceDirection == Vector3.zero) {  return; }

        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.LookRotationDampFactor * Time.deltaTime);
    }
}
