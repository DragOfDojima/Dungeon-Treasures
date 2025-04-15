using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int Damage;
    public float cooldownTime = 1.5f; // Cooldown time in seconds
    private bool canDealDamage = true;
    public bool hitedShield=false;
   
    void Start()
    {


    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided"+other.gameObject.name);
        if(other.tag== "Shield")
        {
            hitedShield=true;
        }
        if (other.tag == "Player" && canDealDamage)
        {
            if (hitedShield == false)
            {
                other.GetComponent<ToPlayer>().getplayer().increaseHp(-Damage);
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
}
