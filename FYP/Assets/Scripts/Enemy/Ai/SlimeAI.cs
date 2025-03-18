using System;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class SlimeAI : MonoBehaviourPun, IPunObservable
{
    public NavMeshAgent agent;
    public float speed;
    [SerializeField] private float knockBackPower;
    public Animator animator;
    private bool playerInSightRange;
    private bool playerInCloseRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    [SerializeField] private SkinnedMeshRenderer bodySkinnedMeshRenderer;
    [SerializeField] private NpcStat npcStat;
    [SerializeField] private GameObject impactDamage;
    private string smile = "smile";
    private string hurt = "hurt";
    private string dead = "dead";
    [SerializeField] private float damage;
    [SerializeField] private AudioClip deadSound;
    private AudioSource audioSource;
    private bool jumpSound;
    private bool isDead;
    private bool startup;
    private float lastHp;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(smile), 100);
        agent.avoidancePriority = 10;
        lastHp = npcStat.getHP();
        npcStat.SetKnockBack(knockBackPower);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sightRange);
    }

    void Update()
    {
        if (!startup)
        {
            lastHp = npcStat.getHP();
            if (lastHp == npcStat.getHP())
            {
                startup = true;
                return;
            }
        }

        if (!npcStat.getDead())
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            if (playerInSightRange) ChasePlayer();
            else Patroling();

            if (lastHp != npcStat.getHP())
            {
                Hurt();
                lastHp = npcStat.getHP();
            }

            lastHp = npcStat.getHP();
            if (animator.enabled)
            {
                if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jumping_baked" && !jumpSound)
                {
                    audioSource.Play();
                    jumpSound = true;
                }
                else
                {
                    jumpSound = false;
                }
            }
        }
        else
        {
            if (!isDead)
            {
                isDead = true;
                gameObject.tag = "Untagged";
                animator.enabled = false;
                audioSource.clip = deadSound;
                audioSource.Play();
                bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(smile), 0);
                bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(hurt), 0);
                bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(dead), 100);
            }
        }

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void ChasePlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;
        playerInCloseRange = Physics.CheckSphere(transform.position, 1.2f, whatIsPlayer);
        if (playerInCloseRange && agent.enabled && animator.enabled)
        {
            agent.updatePosition = false;
            agent.speed = 0;
            agent.updateRotation = false;
            FaceTarget(targetPosition);
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked")
            {
                var id = Instantiate(impactDamage, transform.position, Quaternion.identity) as GameObject;
                id.GetComponent<DealDamage>().setDamage(damage);
            }
        }
        else
        {
            if (agent.enabled && animator.enabled)
            {
                agent.SetDestination(targetPosition);
                agent.updateRotation = true;
                agent.updatePosition = true;
                agent.speed = speed;
            }
        }

        if (agent.enabled)
        {
            agent.SetDestination(targetPosition);
        }

        if (animator.enabled)
            animator.SetBool("jump", true);
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.035f);
    }

    private void Patroling()
    {
        if (agent.enabled)
            agent.speed = 0;

        animator.SetBool("jump", false);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(smile), 100);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(hurt), 0);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(dead), 0);
    }

    private bool hurted;
    public void Hurt()
    {
        sightRange = 100;
        hurted = true;
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(smile), 0);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(hurt), 100);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(dead), 0);
        Invoke("ResetHurt", 2);
    }

    void ResetHurt()
    {
        hurted = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data to other players
            stream.SendNext(npcStat.getHP()); // Current health
            stream.SendNext(isDead); // Dead status
        }
        else
        {
            // Receive data from other players
            lastHp = (float)stream.ReceiveNext(); // Update health from other players
            isDead = (bool)stream.ReceiveNext(); // Update dead status
        }
    }
}