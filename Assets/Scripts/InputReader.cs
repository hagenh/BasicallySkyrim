using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;

    public Action OnJumpPerformed;
    public Action OnInteractPerformed;
    public Action OnTargetPerformed;
    public Action OnTargetCancelPerformed;

    public bool IsAttacking { get; private set; }

    private Controls controls;

    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnInteractPerformed?.Invoke();
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if(!context.performed)
            return;

        OnTargetPerformed?.Invoke();
    }

    public void OnTargetCancel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnTargetCancelPerformed?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true;
        }

        if (context.canceled)
        {
            IsAttacking = false;
        }
    }
}
