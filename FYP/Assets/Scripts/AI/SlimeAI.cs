using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SlimeAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    [SerializeField] private float knockBackPower;
    public Animator animator;
    bool playerInSightRange;
    bool playerInCloseRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    [SerializeField]private SkinnedMeshRenderer bodySkinnedMeshRenderer;
    [SerializeField]private NpcStat npcStat;
    [SerializeField] GameObject impactDamage;
    string Smile = "smile";
    string Hurt = "hurt";
    string Dead = "dead";
    [SerializeField] private float damage;
    [SerializeField] private AudioClip deadSound;
    AudioSource audioSource;
    bool jumpSound;
    bool dead;
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
        Lasthp = npcStat.getHP();
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile), 100);
        agent.avoidancePriority = 10;
        npcStat.SetKnockBack(knockBackPower);

    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sightRange);
    }
    // Update is called once per frame
    float Lasthp;
    void Update()
    {
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
            if (animator.enabled == true)
            {
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
        }
        else
        {
            if (!dead)
            {
                dead=true;
                gameObject.tag = "Untagged";
                animator.enabled = false;
                audioSource.clip = deadSound;
                audioSource.Play();
                bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile), 0);
                bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Hurt), 0);
                bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Dead), 100);
            }
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    bool atk;
    private void ChasePlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;
        playerInCloseRange = Physics.CheckSphere(transform.position, 1.2f, whatIsPlayer);
        if (playerInCloseRange&& agent.enabled == true&& animator.enabled == true)
        {
            agent.updatePosition = false;
            agent.speed = 0;
            agent.updateRotation = false;
            FaceTarget(targetPosition);
            if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked"&&!atk)
            {
                atk=true;
                var id = Instantiate(impactDamage,gameObject.transform.position, Quaternion.identity)as GameObject;
                id.GetComponent<DealDamage>().setDamage(damage);
            }
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Slime_jump2idel_baked")
            {
                atk=false;
            }
        }
        else
        {
            if(agent.enabled == true&& animator.enabled == true)
            {
                agent.SetDestination(targetPosition);
                agent.updateRotation = true;
                agent.updatePosition = true;
                if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_idel2jump_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_Idel_baked")
                {
                    agent.speed = 0;
                }
                else
                {
                    agent.speed = speed;
                }
            }
                
            
            
        }

        if (agent.enabled == true)
        {
            agent.SetDestination(targetPosition);
        }
        if(animator.enabled == true)
            animator.SetBool("jump", true);
        
        if (!hurted)
        {
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile), 0);
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Hurt), 0);
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Dead), 0);
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
    bool roll=false;
    void resetRoll()
    {
        roll = false;
    }
    private void Patroling()
    {
        if(agent.enabled == true)
        agent.speed = 0;
        animator.SetBool("jump", false);
        
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile),100);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Hurt), 0);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Dead), 0);
        if (roll == false)
        {
            roll = true;
            p = UnityEngine.Random.Range(0, 10);
            Invoke("resetRoll",2);
        }
        switch (p)
        {
            case 0:
            case 1:
            
            
                return;
            case 3:
            case 4:
            case 5:
            case 6:
                transform.Rotate(-Vector3.up * 0.08f);
                return;
            case 2:
            case 7:
            case 8:
            case 9:
                transform.Rotate(Vector3.up * 0.08f);
                return;

        }
        
    }
    bool hurted;
    public void hurt()
    {
        hurted=true;
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile), 0);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Hurt), 100);
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Dead), 0);
        Invoke("resetHurt",2);
    }
    void resetHurt()
    {
        hurted=false;
    }
}
