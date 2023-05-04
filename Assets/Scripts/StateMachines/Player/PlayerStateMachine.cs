using UnityEngine;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Vitals))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 Velocity;
    public float MovementSpeed { get; private set; } = 5f;
    public float CrouchSpeed { get; private set; } = 2f;
    public float SprintSpeed { get; private set; } = 7f;
    public float JumpForce { get; private set; } = 5f;
    public Vector3 KnockbackForce { get; private set; }
    public float LookRotationDampFactor { get; private set; } = 10f;
    public Transform MainCamera { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    public Vitals Vitals { get; private set; }
    public Interactable focus;
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReciever ForceReciever { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon{ get; private set; }


    private void Start()
    {
        MainCamera = Camera.main.transform;

        InputReader = GetComponent<InputReader>();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();
        Vitals = GetComponent<Vitals>();
        Targeter = GetComponentInChildren<Targeter>();

        SwitchState(new PlayerMoveState(this));
    }

    void OnDrawGizmos()
    {
        Vector3 cameraCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        Gizmos.DrawSphere(cameraCenter, 0.1f);
    }
}
