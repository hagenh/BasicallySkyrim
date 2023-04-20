using UnityEngine;

public class ForceReciever : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag;

    private Vector3 dampingVelocity;
    private Vector3 impact;

    public Vector3 Forces => impact;

    void Update()
    {
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}
