using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image healthBar;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthBar.fillAmount = fillAmount;
    }
}
