using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SlimeAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public Animator animator;
    bool playerInSightRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    [SerializeField]private SkinnedMeshRenderer bodySkinnedMeshRenderer;
    [SerializeField]private NpcStat npcStat;
    string Smile = "smile";
    string Hurt = "hurt";
    string Dead = "dead";

    // Start is called before the first frame update
    void Start()
    {
        Lasthp = npcStat.getHP();
        bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile), 100);
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
            Vector3 targetPosition = Camera.main.transform.position;
            if (Lasthp != npcStat.getHP())
            {
                hurt();
                Lasthp = npcStat.getHP();
            }
            Lasthp = npcStat.getHP();
        }
        else
        {
            gameObject.tag="Untagged";
            animator.enabled = false;
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile), 0);
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Hurt), 0);
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Dead), 100);
            
        }
        
    }

    private void ChasePlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;

        animator.SetBool("jump", true);
        agent.SetDestination(targetPosition);
        if (!hurted)
        {
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Smile), 0);
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Hurt), 0);
            bodySkinnedMeshRenderer.SetBlendShapeWeight(bodySkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(Dead), 0);
        }
        
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_idel2jump_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_Idel_baked")
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = speed;
        }

    }
    int p;
    bool roll=false;
    void resetRoll()
    {
        roll = false;
    }
    private void Patroling()
    {
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
