using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollLogic : MonoBehaviour
{
    public GameObject ragdollObject;

    public void SpawnRagdoll(Transform enemy)
    {
        Instantiate(ragdollObject, enemy.position, enemy.rotation);
    }
}
