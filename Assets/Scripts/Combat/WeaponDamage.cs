using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;

    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private float damage;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider) { return; }

        if(alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        if (other.gameObject.TryGetComponent(out Vitals vitals))
        {
            Debug.Log("Removing damage from " + other.gameObject.name);
            vitals.DealDamage(damage);
        }
    }

    public void SetAttack(float damage)
    {
        this.damage = damage;
    }
}
