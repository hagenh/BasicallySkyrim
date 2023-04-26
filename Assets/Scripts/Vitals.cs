using UnityEngine;

public class Vitals : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float health;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(float damage)
    {
        if(health <= 0) { return; }

        health = Mathf.Max(health - damage, 0);

        Debug.Log("Took Damage! " + damage);
    }
}
