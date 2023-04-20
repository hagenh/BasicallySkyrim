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
        RaycastHit hit;
        float distance;
        if (Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out hit, Mathf.Infinity, raycastLayerMask))
        {
            distance = hit.distance;
        }
        else
        {
            distance = defaultDistance;
        }

        float scaleDistance = Vector3.Distance(transform.position, targetCamera.transform.position);
        float scaleFactor = scaleDistance / baseDistance;
        transform.localScale = baseScale * scaleFactor * Vector3.one;

        transform.position = targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
    }
}
