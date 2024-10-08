using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStat : MonoBehaviour
{
    [SerializeField] private float Hp;

    public void Damage(float dam)
    {
        Hp-=dam;
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
