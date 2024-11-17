using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float Damage;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "hitable")
        {
            
            var hitPoint = other.ClosestPoint(transform.position);
            Instantiate(hitEffect, hitPoint, Quaternion.identity);
            other.GetComponent<NpcStat>().Damage(Damage);

        }
    }
}
