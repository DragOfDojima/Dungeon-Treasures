using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class KnightAI : MonoBehaviourPun, IPunObservable
{
    public EnemyWeapon enemyWeapon;
    public NavMeshAgent agent;
    public float speed;
    [SerializeField] private float knockBackPower;
    public Animator animator;
    private bool playerInSightRange;
    private bool playerInCloseRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    [SerializeField] private NpcStat npcStat;
    [SerializeField] private int damage;
    [SerializeField] private AudioClip deadSound;
    private AudioSource audioSource;
    private bool dead;
    private bool startup;
    private float Lasthp;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        agent.avoidancePriority = 10;
        Lasthp = npcStat.getHP();
        npcStat.SetKnockBack(knockBackPower);
        enemyWeapon.setDamage(damage);
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
            Lasthp = npcStat.getHP();
            if (Lasthp == npcStat.getHP())
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

            if (Lasthp != npcStat.getHP())
            {
                Hurt();
                Lasthp = npcStat.getHP();
            }
        }
        else
        {
            if (!dead)
            {
                dead = true;
                gameObject.tag = "Untagged";
                animator.enabled = false;
                audioSource.clip = deadSound;
                enemyWeapon.enemyDead();
                animator.SetTrigger("dead");
                audioSource.Play();
            }
        }

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void ChasePlayer()
    {
        animator.SetBool("idel", false);
        Vector3 targetPosition = Camera.main.transform.position;
        agent.SetDestination(targetPosition);
        playerInCloseRange = Physics.CheckSphere(transform.position, 0.8f, whatIsPlayer);

        if (playerInCloseRange && animator.enabled == true)
        {
            int randomValue = Random.Range(0, 100);

            if (randomValue < 70) // 70% chance
            {
                animator.SetBool("atk1", true);
                animator.SetBool("atk2", false);
            }
            else // 30% chance
            {
                animator.SetBool("atk1", false);
                animator.SetBool("atk2", true);
            }

            agent.updatePosition = false;
            agent.speed = 0;
            agent.updateRotation = false;
            agent.enabled = false;
            FaceTarget(targetPosition);
        }
        else
        {
            animator.SetBool("atk1", false);
            animator.SetBool("atk2", false);

            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "walk")
            {
                agent.enabled = true;
                agent.SetDestination(targetPosition);
                agent.updateRotation = true;
                agent.updatePosition = true;
                agent.speed = speed;
            }
        }
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
        agent.speed = 0;
        animator.SetBool("idel", true);
    }

    private bool hurted;
    public void Hurt()
    {
        sightRange = 100;
        hurted = true;
        animator.SetTrigger("damage");
        Invoke("ResetHurt", 2);
    }

    void ResetHurt()
    {
        hurted = false;
        animator.ResetTrigger("damage");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data to other players
            stream.SendNext(npcStat.getHP()); // Current health
            stream.SendNext(dead); // Dead status
        }
        else
        {
            // Receive data from other players
            Lasthp = (float)stream.ReceiveNext(); // Update health from other players
            dead = (bool)stream.ReceiveNext(); // Update dead status
        }
    }
}