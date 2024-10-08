using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (Vector3.Magnitude(rb.velocity) > 10)
            {
                Debug.Log(Vector3.Magnitude(rb.velocity));
                other.GetComponent<NpcStat>().Damage(WeaponDamage+combo*ComboBouns);
                combo++;
                if(combo > MaxCombo)
                {
                    combo=0;
                }
                timer=0;
            }
        }
    }

    float timer=0;
    float time=0;
    private void Update()
    {
        Debug.Log("timer="+timer);
        Debug.Log("time=" + time);
        Debug.Log("speed=" + Vector3.Magnitude(rb.velocity));
        time = Time.time;
        if (combo > 0)
        {
            timer += Time.deltaTime;
            timer -= time;
        }
        if (timer >= 3&&combo>0)
        {
            combo=0;
            timer=0;
        }
        
        
    }
}
