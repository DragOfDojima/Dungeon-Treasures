using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeDamagePerSec : MonoBehaviour
{
    public int Damage=5;
    public float cooldownTime = 1f; // Cooldown time in seconds
    private bool canDealDamage = true;
    ParticleSystem parts;
    float totalDuration;
    private bool canDamage = false;
    public float canDamageStartTime = 0f;


    void Start()
    {
        parts = GetComponent<ParticleSystem>();
        totalDuration = parts.main.duration;
        Destroy(gameObject, totalDuration);
        StartCoroutine(DamageStartTime());
    }
    private IEnumerator DamageStartTime()
    {
        yield return new WaitForSeconds(canDamageStartTime);
        canDamage = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!canDamage) return;

        Debug.Log("collided" + other.gameObject.name);
        if (other.tag == "Player" && canDealDamage)
        {
            other.GetComponent<ToPlayer>().getplayer().increaseHp(-Damage);
            StartCoroutine(DamageCooldown());

        }

    }
    private IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(cooldownTime);
        canDealDamage = true;
    }
    // Update is called once per frame
    public void setDamage(int d)
    {
        Damage = d;
    }

    public void enemyDead()
    {
        Damage = 0;
    }

    
}
