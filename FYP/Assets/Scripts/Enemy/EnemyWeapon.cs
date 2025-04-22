using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int Damage;
    public float cooldownTime = 1.5f; // Cooldown time in seconds
    private bool canDealDamage = true;
    private bool hitedShield=false;
    public AudioSource audioSource;
   
    void Start()
    {


    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided"+other.gameObject.name);
        if(other.tag== "Shield")
        {
            other.GetComponent<weapon>().shield();
            hitedShield = true;
        }
        if (other.tag == "Player" && canDealDamage)
        {
            if (hitedShield == false)
            {
                other.GetComponent<ToPlayer>().getplayer().increaseHp(-Damage);
                audioSource.Play();
                StartCoroutine(DamageCooldown());
            }
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
        Damage=0;
    }

    public void setHitedShield()
    {
        hitedShield=false;
    }
}
