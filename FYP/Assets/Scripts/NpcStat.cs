using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStat : MonoBehaviour
{
    [SerializeField] private float MaxHp;
    private float Hp;
    private Object floatDam;
    [SerializeField] private float floatDamOffset=0.5f;
    bool iframe= false;
    [SerializeField] private HealthBar healthBar;

    private void Start()
    {
        Hp = MaxHp;
        healthBar.UpdateHealthBar(MaxHp,Hp);
        floatDam = Resources.Load("damageText");
    }
    public void Damage(float dam)
    {
        if (!iframe)
        {
            int Damage = (int)Mathf.Ceil(dam);
            iframe = true;
            Hp -= Damage;
            var floatdam = Instantiate(floatDam, transform.position, transform.rotation)as GameObject;
            Debug.Log(floatdam);
            Debug.Log(floatdam.GetComponent<floattext>());
            floatdam.GetComponent<floattext>().setText(Damage);
            floatdam.GetComponent<floattext>().setOffset(floatDamOffset);
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
