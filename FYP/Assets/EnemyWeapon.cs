using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    int Damage;
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided"+other.gameObject.name);
        if (other.tag == "Player")
        {
            other.GetComponent<ToPlayer>().getplayer().increaseHp(-Damage);
        }
        
    }
    // Update is called once per frame
    public void setDamage(int d)
    {
        Damage = d;
    }
}
