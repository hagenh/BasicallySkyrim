using UnityEngine;

public class ForceReciever : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerStateMachine StateMachine;
    [SerializeField] private float drag;

    private Vector3 dampingVelocity;
    private Vector3 impact;
    public Vector3 Forces => impact;

    void Update()
    {
        //impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        // Slowly reduce the Vector 3 impact over time
        impact = Vector3.Lerp(impact, Vector3.zero, 3f * Time.deltaTime);

    }

    public void AddForce(PlayerStateMachine stateMachine, Vector3 force)
    {
        impact += force;
        stateMachine.Controller.Move(impact * Time.deltaTime);
    }
}
