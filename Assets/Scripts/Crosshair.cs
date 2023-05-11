using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Camera targetCamera;
    public LayerMask raycastLayerMask;

    private float baseScale = 0.05f;
    private float baseDistance = 10f;
    private readonly float defaultDistance = 10f;

    void Update()
    {
        var transform1 = targetCamera.transform;
        var distance = Physics.Raycast(transform1.position, transform1.forward, out var hit, Mathf.Infinity, raycastLayerMask) ? hit.distance : defaultDistance;

        var scaleDistance = Vector3.Distance(transform.position, targetCamera.transform.position);
        var scaleFactor = scaleDistance / baseDistance;
        transform.localScale = baseScale * scaleFactor * Vector3.one;

        transform.position = targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
    }
}
