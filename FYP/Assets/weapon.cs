using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    [SerializeField] private float WeaponDamage;
    [SerializeField] private Collider hitbox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hitable")
        {

        }
    }
}
