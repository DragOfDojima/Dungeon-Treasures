using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

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
    private float knockbackPower;
    SkinnedMeshRenderer smr;
    Material[] deadmatList;
    public GameObject NPC;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Dead());
        }
    }
    private void Start()
    {
        smr = MainObject.GetComponent<SkinnedMeshRenderer>();
        //deadanimation.enabled = true;
        //Destroy(MainObject.GetComponent<SkinnedMeshRenderer>().material);
        CurrentHP = Hp;
        healthBar.UpdateHealthBar(CurrentHP, Hp);
        floatDam = Resources.Load("damageText");
        deadmatList = smr.materials;
        for (int i = 0; i < deadmatList.Length; i++)
        {
            deadmatList[i] = deadMat;
        }
    }
    public void Damage(float dam)
    {
        if (!deaded)
        {
            if (!iframe)
            {
                int Damage = (int)Mathf.Ceil(dam);
                StartCoroutine(ApplyKnockback(transform.forward*knockbackPower));
                iframe = true;
                CurrentHP -= Damage;
                var floatdam = Instantiate(floatDam, transform.position, transform.rotation) as GameObject;
                floatdam.GetComponent<floattext>().setText(Damage);
                floatdam.GetComponent<floattext>().setOffset(floatDamOffset);
                healthBar.UpdateHealthBar(CurrentHP, Hp);
                if (CurrentHP <= 0)
                {
                    StartCoroutine(Dead());
                    GameObject.Find("MobSpawner").GetComponent<Mobspawner>().killedMob();
                }
                NPC.GetComponent<NavMeshAgent>().enabled = false;
                MainObject.GetComponent<Animator>().enabled = false;
                StartCoroutine(iframeEnd());
            }
        }
        
        
    }
    IEnumerator iframeEnd()
    {
        yield return new WaitForSeconds(0.6f);
        NPC.GetComponent<NavMeshAgent>().enabled = true;
        MainObject.GetComponent<Animator>().enabled = true;
        iframe = false;
    }
    public float getHP()
    {
        return CurrentHP;
    }
    
    bool deaded;
    IEnumerator Dead()
    {
        deaded=true;
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        smr.materials = deadmatList;
        deadanimation.enabled = true;
        yield return new WaitForSeconds(2f);
        Destroy(NPC);
    }

  
    public bool getDead()
    {
        return deaded;
    }
    private IEnumerator ApplyKnockback(Vector3 force)
    {
        yield return null;
        NPC.GetComponent<NavMeshAgent>().enabled = false;
        NPC.GetComponent<Rigidbody>().useGravity = true;
        NPC.GetComponent<Rigidbody>().isKinematic = false;
        NPC.GetComponent<Rigidbody>().AddForce(force);

        yield return new WaitForFixedUpdate();
        float knockbackTime = Time.time;
        yield return new WaitUntil(
            () => NPC.GetComponent<Rigidbody>().velocity.magnitude < 0.05f || Time.time > knockbackTime + 0.5f
        );
        yield return new WaitForSeconds(0.25f);

        NPC.GetComponent<Rigidbody>().velocity = Vector3.zero;
        NPC.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        NPC.GetComponent<Rigidbody>().useGravity = false;
        NPC.GetComponent<Rigidbody>().isKinematic = true;
        NPC.GetComponent<NavMeshAgent>().Warp(transform.position);
        NPC.GetComponent<NavMeshAgent>().enabled = true;

        yield return null;
    }

    public void SetKnockBack(float k)
    {
        knockbackPower = k;
    }

    public void kys()
    {
        Damage(CurrentHP);
    }
}
