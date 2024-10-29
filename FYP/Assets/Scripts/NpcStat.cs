using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class NpcStat : MonoBehaviour
{
    [SerializeField] private float Hp;
    private float CurrentHP;
    private Object floatDam;
    [SerializeField] private float floatDamOffset=0.5f;
    bool iframe= false;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Material deadMat;
    [SerializeField] private GameObject MainObject;
    [SerializeField] private Animator deadanimation;
    private void Start()
    {
        //deadanimation.enabled = true;
        //Destroy(MainObject.GetComponent<SkinnedMeshRenderer>().material);
        CurrentHP = Hp;
        healthBar.UpdateHealthBar(CurrentHP, Hp);
        floatDam = Resources.Load("damageText");
    }
    public void Damage(float dam)
    {
        if (!iframe)
        {
            int Damage = (int)Mathf.Ceil(dam);
            iframe = true;
            CurrentHP -= Damage;
            var floatdam = Instantiate(floatDam, transform.position, transform.rotation)as GameObject;
            Debug.Log(floatdam);
            Debug.Log(floatdam.GetComponent<floattext>());
            floatdam.GetComponent<floattext>().setText(Damage);
            floatdam.GetComponent<floattext>().setOffset(floatDamOffset);
            healthBar.UpdateHealthBar(CurrentHP, Hp);
            if (CurrentHP <= 0)
            {
                StartCoroutine(Dead());
                GameObject.Find("MobSpawner").GetComponent<Mobspawner>().killedMob();
            }
            Invoke("iframeEnd", 0.3f);  
        }
        
    }

    public float getHP()
    {
        return CurrentHP;
    }
    private void iframeEnd()
    {
        iframe = false;
    }
    bool deaded;
    IEnumerator Dead()
    {
        deaded=true;
        
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    public bool getDead()
    {
        return deaded;
    }
    private void Update()
    {
        //Debug.Log(MainObject.GetComponent<SkinnedMeshRenderer>().materials[1]);

    }

}
