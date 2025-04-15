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

    private GameObject owner;
    [SerializeField] private GameObject[] allPlayer;



    private void Start()
    {
        audioSource=GetComponent<AudioSource>();
        allPlayer = GameObject.FindGameObjectsWithTag("PlayerGO");
        owner = allPlayer[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "hitable")
        {
            if (speed > 2)
            {
                var hitPoint = other.ClosestPoint(transform.position);
                Instantiate(hitEffect, hitPoint, Quaternion.identity);
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
                float dealDamage = WeaponDamage + combo * ComboBouns;
                other.GetComponent<NpcStat>().setHitByWho(owner);
                other.GetComponent<NpcStat>().Damage(dealDamage);
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
        if (gameObject.GetComponent<MyGrabable>().getIsGrabing())
        {
            GameObject nearestPlayer = allPlayer[0];
            float distanceToNearest = Vector3.Distance(transform.position, nearestPlayer.transform.position);
            for (int i = 0; i < allPlayer.Length; i++)
            {
                float distanceToCurrent = Vector3.Distance(transform.position, allPlayer[i].transform.position);
                if (distanceToCurrent < distanceToNearest)
                {
                    nearestPlayer = allPlayer[i];
                    distanceToNearest = distanceToCurrent;
                }
            }
            owner = nearestPlayer;
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

    float speed;
    private void FixedUpdate()
    {
        speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        
    }

}
