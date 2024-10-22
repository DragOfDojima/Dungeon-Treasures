using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image healthBar;
    [SerializeField] private float reduceSpeed = 2;
    private float target = 1;

    public void UpdateHealthBar(float currentHealth, float maxHealth){
    
        float fillAmount = (float)currentHealth / maxHealth;
        target = fillAmount;
    }

    private void Update(){
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, target, reduceSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

    }
}
