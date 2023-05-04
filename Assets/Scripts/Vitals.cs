using UnityEngine;

public class Vitals : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;

    public float health;

    public delegate void EntityDied();
    public event EntityDied OnDied;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(float damage)
    {
        if(health <= 0) { return; }

        health = Mathf.Max(health - damage, 0);

        OnDied?.Invoke();
    }
}
