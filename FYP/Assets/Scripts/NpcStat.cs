using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStat : MonoBehaviour
{
    [SerializeField] private float Hp;
    [SerializeField] private GameObject floatDam;
    [SerializeField] private int floatDamOffset;
    bool iframe= false;
    public void Damage(float dam)
    {
        if (!iframe)
        {
            iframe = true;
            Hp -= dam;
            GameObject floatdam = Instantiate(floatDam, transform.position, transform.rotation);
            floatdam.GetComponent<floattext>().setText(dam);
            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
            Invoke("iframeEnd", 0.3f);
        }
        
    }

    private void iframeEnd()
    {
        iframe = false;
    }
}
