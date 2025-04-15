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
    [SerializeField] private AudioClip deadSound;
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
                audioSource.clip = deadSound;
                enemyWeapon.enemyDead();
                animator.SetTrigger("dead");
                audioSource.Play();
            }
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    bool atk;
    private void ChasePlayer()
    {
        animator.SetBool("idel", false);
        Vector3 targetPosition = Camera.main.transform.position;
        if(agent.isActiveAndEnabled)
        agent.SetDestination(targetPosition);
        playerInCloseRange = Physics.CheckSphere(transform.position, 1.2f, whatIsPlayer);
        if (playerInCloseRange && animator.enabled == true)
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
        animator.SetTrigger("damage");
        Invoke("resetHurt", 2);
    }
    void resetHurt()
    {
        hurted = false;
        agent.speed = speed;
        animator.ResetTrigger("damage");
    }
}
