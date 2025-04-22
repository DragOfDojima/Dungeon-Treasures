using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class KnightAI : MonoBehaviour
{

    public EnemyWeapon enemyWeapon;
    public NavMeshAgent agent;
    public float speed;
    [SerializeField] private float knockBackPower;
    public Animator animator;
    bool playerInSightRange;
    bool playerInCloseRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    [SerializeField] private NpcStat npcStat;
    [SerializeField] private int damage;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private AudioClip walkSound;
    AudioSource audioSource;
    bool dead;
    bool startup;
    float Lasthp;
    // Start is called before the first frame update

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
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sightRange);
    }
    // Update is called once per frame

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
                hurt();
                Lasthp = npcStat.getHP();
            }
            Lasthp = npcStat.getHP();
            
        }
        else
        {
            if (!dead)
            {
                dead = true;
                gameObject.tag = "Untagged";
                animator.enabled = false;
                audioSource.clip = impactSound;
                enemyWeapon.enemyDead();
                animator.SetTrigger("dead");
                audioSource.loop = false;
                audioSource.Play();
            }
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    bool atk;
    bool played;
    private void ChasePlayer()
    {
        animator.SetBool("idel", false);
        audioSource.clip = walkSound;
        audioSource.loop = true;
        if (!played)
        {
            audioSource.Play();
            played = true;
        }
        Vector3 targetPosition = Camera.main.transform.position;
        if(agent.isActiveAndEnabled)
        agent.SetDestination(targetPosition);
        playerInCloseRange = Physics.CheckSphere(transform.position, 1.4f, whatIsPlayer);
        enemyWeapon.setHitedShield();
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("slash1")|| animator.GetCurrentAnimatorStateInfo(0).IsName("slash2"))
        //  return;
        if (playerInCloseRange)
        {
            audioSource.Pause();

        }
        
        
        if (playerInCloseRange && agent.enabled == true)
        {
            agent.updatePosition = false;
            agent.speed = 0;
            agent.updateRotation = false;
            agent.enabled = false;
            int randomValue = UnityEngine.Random.Range(0, 100);
            // Set animator parameters based on the random value
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
            FaceTarget(targetPosition);
        }
        else
        {
            animator.SetBool("atk1", false);
            animator.SetBool("atk2", false);
            Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);

            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name=="walk")
            {
                audioSource.UnPause();

                agent.enabled = true;
                agent.SetDestination(targetPosition);
                agent.updateRotation = true;
                agent.updatePosition = true;
                agent.speed = speed;
            }
        }
        

        if (!hurted)
        {

        }
    }
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.035f);
    }
    int p;
    bool roll = false;
    void resetRoll()
    {
        roll = false;
    }
    private void Patroling()
    {
        if (agent.enabled == true)
            agent.speed = 0;
        animator.SetBool("idel", true);
        
    }
    bool hurted;
    public void hurt()
    {
        sightRange = 100;
        hurted = true;
        agent.speed = 0;
        audioSource.clip = impactSound;
        audioSource.loop = false;
        played = false;
        audioSource.Play();
        if (hurted == false)
        {
            animator.SetTrigger("damage");
            Invoke("resetHurt", 2);
        }
        
    }
    void resetHurt()
    {
        hurted = false;
        agent.speed = speed;
        animator.ResetTrigger("damage");
    }
}
