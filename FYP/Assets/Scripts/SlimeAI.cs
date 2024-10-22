using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
public class SlimeAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public  float speed;
    public Animator animator;
    public LayerMask whatIsPlayer;
    public float sightRange;
    public bool playerInSightRange;
    void Start()
    {
        agent.angularSpeed=360;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if(playerInSightRange ) ChasePlayer();
        else Patroling();
        
    }
    private void ChasePlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;
        animator.SetBool("jump",true);
        agent.SetDestination(targetPosition);
        
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name== "Slime_idel2jump_baked"|| animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_Idel_baked")
        {
           agent.speed = 0;
        }
        else { agent.speed = speed; }
    }

    private void Patroling()
    {
        animator.SetBool("jump", false);
        agent.speed = 0;
    }
}
