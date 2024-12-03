using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class weapon : MonoBehaviour
{
    [SerializeField] private float WeaponDamage;
    [SerializeField] private float ComboBouns;
    [SerializeField] private int MaxCombo;
    [SerializeField] private GameObject hitEffect;

    AudioSource audioSource;
    private int combo = 0;

    private void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "hitable")
        {
            if (speed > 5)
            {
                var hitPoint = other.ClosestPoint(transform.position);
                Instantiate(hitEffect, hitPoint, Quaternion.identity);
                audioSource.Play();
                other.GetComponent<NpcStat>().Damage(WeaponDamage+combo*ComboBouns);
                if(combo<MaxCombo)
                combo++;
                timer=3;
            }
        }
    }
    float timer=3;
    Vector3 lastPosition = Vector3.zero;
    private void Update()
    {
        
        //Debug.Log("c=" + combo);
        if (combo > 0)
        {
            timer -= Time.deltaTime;
        }
        
        if (timer <= 0.0f && combo > 0)
        {
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
