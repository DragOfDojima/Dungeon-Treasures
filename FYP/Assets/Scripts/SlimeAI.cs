using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
public class SlimeAI : MonoBehaviour
{
    public NavMeshAgent agent;
<<<<<<< Updated upstream
    public  float speed;
=======
    public float speed;
>>>>>>> Stashed changes
    public Animator animator;
    public LayerMask whatIsPlayer;
    public float sightRange;
    public bool playerInSightRange;
    void Start()
    {
<<<<<<< Updated upstream
        agent.angularSpeed=360;
=======
        agent.angularSpeed = 360;
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
<<<<<<< Updated upstream
        if(playerInSightRange ) ChasePlayer();
        else Patroling();
        
=======
        if (playerInSightRange) ChasePlayer();
        else Patroling();

>>>>>>> Stashed changes
    }
    private void ChasePlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;
<<<<<<< Updated upstream
        animator.SetBool("jump",true);
        agent.SetDestination(targetPosition);
        
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name== "Slime_idel2jump_baked"|| animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_Idel_baked")
        {
           agent.speed = 0;
=======
        animator.SetBool("jump", true);
        agent.SetDestination(targetPosition);

        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_idel2jump_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_Idel_baked")
        {
            agent.speed = 0;
>>>>>>> Stashed changes
        }
        else { agent.speed = speed; }
    }

    private void Patroling()
    {
        animator.SetBool("jump", false);
        agent.speed = 0;
    }
}
