using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyStateMachine : StateMachine
{
    [Header("Enemy Movement")]
    public Vector3 Velocity;
    public float MovementSpeed { get; private set; } = 1f;
    public float RotationSpeed { get; private set; } = 5f;
    public float JumpForce { get; private set; } = 5f;

    public float viewDistance = 999999f;
    public float fieldOfView = 90f;

    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Character_Knight 1");
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
