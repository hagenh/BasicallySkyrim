using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Vitals vitals;
    public Image backgroundHealthBar;
    public Image healthBar;

    private void Update()
    {
        // Update the health bar's color and position based on the enemy's health
        float fillAmount = vitals.health / vitals.maxHealth;
        healthBar.fillAmount = fillAmount;

        // Position the health bar above the enemy's head
        Vector3 worldPos = vitals.transform.position + new Vector3(0, 2, 0);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthBar.transform.position = screenPos;
        backgroundHealthBar.transform.position = screenPos;
    }
}
