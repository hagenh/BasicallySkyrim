using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private Camera mainCamera;

    public List<Target> targets = new();
    public Target currentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) { return; }

        targets.Add(target);

        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target target)){ return; }

        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (currentTarget != null) { cineTargetGroup.RemoveMember(currentTarget.transform); }

        if (targets.Count < 1) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach(Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) { continue; }

            Vector2 toCenter = new Vector2(0.5f, 0.5f) - viewPos;
            if(toCenter.sqrMagnitude < closestTargetDistance) 
            { 
                closestTarget = target; 
                closestTargetDistance = toCenter.sqrMagnitude;
            }

        }

        if(closestTarget == null) { return false; }

        currentTarget = closestTarget;
        cineTargetGroup.AddMember(currentTarget.transform, 1f, 1.25f);

        return true;
    }

    public void Cancel()
    {
        if(currentTarget == null) { return; }

        cineTargetGroup.RemoveMember(currentTarget.transform);

        currentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if(currentTarget == target)
        {
            cineTargetGroup.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
