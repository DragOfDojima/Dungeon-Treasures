using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class weapon : MonoBehaviour
{
    [SerializeField] private float WeaponDamage;
    [SerializeField] private Collider hitbox;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float ComboBouns;
    [SerializeField] private int MaxCombo;
    private int combo = 0;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "hitable")
        {
            if (speed > 5)
            {
                other.GetComponent<NpcStat>().Damage(WeaponDamage+combo*ComboBouns);
                combo++;
                if(combo > MaxCombo)
                {
                    combo=0;
                }
                timer=3;
            }
        }
    }

    float timer=3;
    Vector3 lastPosition = Vector3.zero;
    private void Update()
    {
        Debug.Log("timer="+timer);
        Debug.Log("c=" + combo);
        if (combo > 0)
        {
            timer -= Time.time;
        }
        
        if (timer <= 0.0f && combo > 0)
        {
            Debug.Log("WWWW");
            combo=0;
            timer = 3;
        }
        
        
        
    }
    float speed;
    private void FixedUpdate()
    {
        speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

    }
}
