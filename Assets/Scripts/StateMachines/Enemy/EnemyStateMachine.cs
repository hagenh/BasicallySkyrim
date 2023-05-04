using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyStateMachine : StateMachine
{
    public Animator Animator { get; private set; }
    public Vitals Vitals { get; private set; }
    public Collider Collider { get; set; }
    public RagdollLogic RagdollLogic { get; private set; }

    [Header("Enemy Movement")]
    public NavMeshAgent navMeshAgent;
    public Vector3 Velocity;
    public float MovementSpeed { get; private set; } = 1f;
    public float MovementRunSpeed { get; private set; } = 3f;
    public float RotationSpeed { get; private set; } = 5f;
    public float JumpForce { get; private set; } = 5f;


    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }

    public float viewDistance = 999999f;
    public float fieldOfView = 90f;

    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public GameObject player;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        player = GameObject.Find("Character_Knight 1");
        navMeshAgent = GetComponent<NavMeshAgent>();
        Vitals = GetComponent<Vitals>();
        Collider = GetComponent<Collider>();
        RagdollLogic = GetComponent<RagdollLogic>();

        SwitchState(new EnemyPatrolState(this));
    }

    void OnDrawGizmos()
    {
        Vector3 leftEdge = Quaternion.Euler(0, -fieldOfView / 2f, 0) * transform.forward * viewDistance;
        Vector3 rightEdge = Quaternion.Euler(0, fieldOfView / 2f, 0) * transform.forward * viewDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftEdge);
        Gizmos.DrawLine(transform.position, transform.position + rightEdge);
    }
}
