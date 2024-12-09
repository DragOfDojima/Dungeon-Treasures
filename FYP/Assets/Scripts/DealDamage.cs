using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] float dealDanage;

    public void setDamage(float d)
    {
        dealDanage = d;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"&&other.name=="Body")
        {
            other.GetComponent<ToPlayer>().getplayer().increaseHp(-dealDanage);
            Destroy(gameObject);
        }else
            Destroy(gameObject);
    }

    private void Start()
    {
        Destroy(gameObject,0.5f);
    }
}
