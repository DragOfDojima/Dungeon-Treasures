using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int Damage;
    public float cooldownTime = 1f; // Cooldown time in seconds
    private bool canDealDamage = true;

   
    void Start()
    {


    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided"+other.gameObject.name);
        if(other.tag== "Shield")return;
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
        Damage=0;
    }
}
