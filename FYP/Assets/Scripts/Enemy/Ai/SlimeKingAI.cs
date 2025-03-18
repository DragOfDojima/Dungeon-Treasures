using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class SlimeKingAI : MonoBehaviourPun, IPunObservable
{
    public NavMeshAgent agent;
    public float speed;
    [SerializeField] private float knockBackPower;
    public Animator animator;
    public GameObject slimeKingAtk;
    public GameObject slimeKingAtkSpawner;
    [SerializeField] private NpcStat npcStat;
    private bool playerInCloseRange;
    public LayerMask whatIsPlayer;
    [SerializeField] private GameObject impactDamage;
    [SerializeField] private float damage;
    private AudioSource audioSource;
    [SerializeField] private AudioClip deadSound;
    private bool jumpSound;
    private bool dead;
    private float lastHp;
    private bool attacking = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator.SetBool("jump", true);
        lastHp = npcStat.getHP();
        agent.avoidancePriority = 10;
        agent.speed = speed;
        npcStat.SetKnockBack(knockBackPower);
    }

    void Update()
    {
        if (!npcStat.getDead())
        {
            AtkPlayer();
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jumping_baked")
            {
                if (!jumpSound)
                {
                    audioSource.Play();
                    jumpSound = true;
                }
            }
            else
            {
                jumpSound = false;
            }
        }
        else
        {
            if (!dead)
            {
                dead = true;
                audioSource.clip = deadSound;
                audioSource.Play();
                gameObject.tag = "Untagged";
                animator.enabled = false;
            }
        }
    }

    private void AtkPlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;

        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jumping_baked" && !attacking && !npcStat.getDead())
        {
            attacking = true;
            StartCoroutine(Attack());
        }

        playerInCloseRange = Physics.CheckSphere(transform.position, 3f, whatIsPlayer);
        if (playerInCloseRange)
        {
            agent.updatePosition = false;
            agent.speed = 0;
            agent.updateRotation = false;
            FaceTarget(targetPosition);
        }
        else
        {
            if (agent.enabled)
            {
                agent.SetDestination(targetPosition);
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.speed = speed;
            }
        }

        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked" && !attacking)
        {
            attacking = true;
            var id = Instantiate(impactDamage, transform.position, Quaternion.identity) as GameObject;
            id.GetComponent<SphereCollider>().radius = 2.5f;
            id.GetComponent<DealDamage>().setDamage(damage);
        }

        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Slime_jump2idel_baked")
        {
            attacking = false;
        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.035f);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.1f);
        CreateSlimeKingAtk(8, slimeKingAtkSpawner.transform.position, 0.85f);
    }

    public void CreateSlimeKingAtk(int num, Vector3 point, float radius)
    {
        float randomRotationOffset = Random.Range(0f, 60f * Mathf.Deg2Rad);
        for (int i = 0; i < num; i++)
        {
            var radians = 2 * Mathf.PI / num * i + randomRotationOffset;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = point + spawnDir * radius;

            var enemy = Instantiate(slimeKingAtk, spawnPos, Quaternion.identity) as GameObject;
            enemy.transform.LookAt(point);
            enemy.transform.eulerAngles = new Vector3(enemy.transform.eulerAngles.x + 25, enemy.transform.eulerAngles.y, enemy.transform.eulerAngles.z);
            enemy.transform.Translate(new Vector3(0, enemy.transform.localScale.y / 2, 0));
            enemy.GetComponent<Rigidbody>().AddForce(-enemy.transform.forward * 500);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(npcStat.getHP()); // Send current health
            stream.SendNext(dead); // Send dead status
        }
        else
        {
            lastHp = (float)stream.ReceiveNext(); // Receive health
            dead = (bool)stream.ReceiveNext(); // Receive dead status
        }
    }
}