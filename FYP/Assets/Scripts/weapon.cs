using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class weapon : MonoBehaviour
{
    Animator animator;
    [SerializeField] private float WeaponDamage;
    [SerializeField] private Collider hitbox;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float ComboBouns;
    [SerializeField] private int MaxCombo;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private Grabbable grabbable;

    private int combo = 0;


    void Start()
    {
        animator = GetComponent<Animator>();

    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "hitable")
        {
            if (speed > 5)
            {
                var hitPoint = other.ClosestPoint(transform.position);
                Instantiate(hitEffect, hitPoint, Quaternion.identity);
                other.GetComponent<NpcStat>().Damage(WeaponDamage+combo*ComboBouns);
                if(combo<MaxCombo)
                combo++;
                timer=3;
            }
        }
    }
    bool isgrabing;
    bool setup;
    bool setup2;
    float timer=3;
    Vector3 lastPosition = Vector3.zero;
    private void Update()
    {
        if(grabbable.SelectingPoints.Count > 0)
        {
            isgrabing = true;
            if (!setup)
            {
                setup = true;
                Setup();
            }
        }
        else
        {
            isgrabing= false;
            if (setup&&!setup2)
            {
                setup2 = true;
                Setup();
            }
        }
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

    void Setup()
    {
        animator.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        hitbox.isTrigger = false;
    }
    float speed;
    private void FixedUpdate()
    {
        speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        
    }

}
