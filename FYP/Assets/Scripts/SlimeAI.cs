using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SlimeAI : MonoBehaviour
{
    public NavMeshAgent agent;
<<<<<<< HEAD
<<<<<<< Updated upstream
    public  float speed;
=======
    public float speed;
>>>>>>> Stashed changes
=======
    public  float speed=1;
>>>>>>> parent of 37f27d1 (new ai)
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
<<<<<<< Updated upstream
        agent.angularSpeed=360;
=======
        agent.angularSpeed = 360;
>>>>>>> Stashed changes
=======
        
>>>>>>> parent of 37f27d1 (new ai)
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
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
=======
        Vector3 targetPosition =  Camera.main.transform.position;
>>>>>>> parent of 37f27d1 (new ai)

        agent.SetDestination(targetPosition);
        agent.speed = speed;
    }
}
