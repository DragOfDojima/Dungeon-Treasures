using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStat : MonoBehaviour
{
    [SerializeField] private float Hp;

    bool iframe;
    public void Damage(float dam)
    {
        iframe = true;
        Debug.Log(dam);
        Hp-=dam;
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
        Invoke("iframeEnd",0.3f);
    }

    private void iframeEnd()
    {
        iframe = false;
    }
}
