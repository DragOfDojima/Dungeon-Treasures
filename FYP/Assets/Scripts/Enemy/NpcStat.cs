using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class NpcStat : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float Hp;
    private float CurrentHP;
    private Object floatDam;
    [SerializeField] private float floatDamOffset = 0.5f;
    private bool iframe = false;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Material deadMat;
    [SerializeField] private GameObject MainObject;
    [SerializeField] private Animator deadanimation;
    [SerializeField] private Collider[] colliders;
    private float knockbackPower;
    private SkinnedMeshRenderer smr;
    private Material[] deadmatList;
    public GameObject NPC;
    [SerializeField] private int Score;
    private Player hitByWho;
    private bool deaded;

    private void Start()
    {
        smr = MainObject.GetComponent<SkinnedMeshRenderer>();
        CurrentHP = Hp;
        healthBar.UpdateHealthBar(CurrentHP, Hp);
        floatDam = Resources.Load("damageText");

        if (smr != null)
        {
            deadmatList = smr.materials;
            for (int i = 0; i < deadmatList.Length; i++)
            {
                deadmatList[i] = deadMat;
            }
        }
    }

    public void Damage(float dam)
    {
        if (!deaded && photonView.IsMine) // Only the master client handles damage
        {
            if (!iframe)
            {
                int Damage = (int)Mathf.Ceil(dam);
                StartCoroutine(ApplyKnockback(transform.forward * knockbackPower));
                iframe = true;
                CurrentHP -= Damage;
                var floatdam = Instantiate(floatDam, transform.position, transform.rotation) as GameObject;
                floatdam.GetComponent<floattext>().setText(Damage);
                floatdam.GetComponent<floattext>().setOffset(floatDamOffset);
                healthBar.UpdateHealthBar(CurrentHP, Hp);

                if (hitByWho == null)
                {
                    hitByWho = GameObject.FindGameObjectWithTag("PlayerGO").GetComponent<Player>();
                }
                hitByWho.AddDealDamage(Damage);

                if (CurrentHP <= 0)
                {
                    photonView.RPC("DeadRPC", RpcTarget.All);
                }

                NPC.GetComponent<NavMeshAgent>().enabled = false;
                MainObject.GetComponent<Animator>().enabled = false;
                StartCoroutine(iframeEnd());
            }
        }
    }

    [PunRPC]
    void DeadRPC()
    {
        if (hitByWho == null)
        {
            hitByWho = GameObject.FindGameObjectWithTag("PlayerGO").GetComponent<Player>();
        }
        hitByWho.AddScore(Score);
        hitByWho.AddEnemySlayed(1);
        GameObject.Find("MobSpawner").GetComponent<Mobspawner>().killedMob();
        deaded = true;

        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
        healthBar.gameObject.SetActive(false);

        if (smr != null)
            smr.materials = deadmatList;

        if (deadanimation != null)
            deadanimation.enabled = true;

        Destroy(NPC, 3f); // Destroy after 3 seconds
    }

    IEnumerator iframeEnd()
    {
        yield return new WaitForSeconds(0.6f);
        NPC.GetComponent<NavMeshAgent>().enabled = true;
        MainObject.GetComponent<Animator>().enabled = true;
        iframe = false;
    }

    public float getHP() => CurrentHP;
    public bool getDead() => deaded;

    private IEnumerator ApplyKnockback(Vector3 force)
    {
        yield return null;
        NPC.GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody rb = NPC.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(force);

        yield return new WaitForFixedUpdate();
        float knockbackTime = Time.time;
        yield return new WaitUntil(() => rb.velocity.magnitude < 0.05f || Time.time > knockbackTime + 0.5f);
        yield return new WaitForSeconds(0.25f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
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

    public int getScore() => Score;

    public void setHitByWho(GameObject hbw)
    {
        hitByWho = hbw.GetComponent<Player>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentHP);
            stream.SendNext(deaded);
        }
        else
        {
            CurrentHP = (float)stream.ReceiveNext();
            deaded = (bool)stream.ReceiveNext();
        }
    }
}