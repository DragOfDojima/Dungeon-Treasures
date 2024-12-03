using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SlimeKingAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    [SerializeField] private float knockBackPower;
    public Animator animator;
    public GameObject slimeKingAtk;
    public GameObject slimeKingAtkSpawner;
    [SerializeField]private NpcStat npcStat;
    bool playerInCloseRange;
    public LayerMask whatIsPlayer;
    [SerializeField] GameObject impactDamage;
    [SerializeField] private float damage;
    AudioSource audioSource;
    bool jumpSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
        animator.SetBool("jump", true);
        //agent.updatePosition = false;
        Lasthp = npcStat.getHP();
        agent.avoidancePriority = 10;
        agent.speed = speed;
        npcStat.SetKnockBack(knockBackPower);
    }

    // Update is called once per frame
    float Lasthp;
    bool atking=false;
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
            gameObject.tag="Untagged";
            animator.enabled = false; 
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
        }

        

    }
    bool atk;
    IEnumerator attack()
    {
        yield return new WaitForSeconds(1.1f);
        CreateSlimeKingAtk(8, slimeKingAtkSpawner.transform.position, 0.85f);
    }
    private void AtkPlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;
        
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jumping_baked" && !atking && !npcStat.getDead())
        {
            atking = true;
            StartCoroutine(attack());
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
            if (agent.enabled == true)
            {
                agent.SetDestination(targetPosition);
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.speed = speed;
            }
            
        }

        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jump2idel_baked" && !atk)
        {
            atk = true;
            var id = Instantiate(impactDamage, gameObject.transform.position, Quaternion.identity) as GameObject;
            id.GetComponent<DealDamage>().setDamage(damage);
        }
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Slime_jump2idel_baked")
        {
            atk = false;
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
        
    }
    bool hurted;
    public void hurt()
    {
        hurted=true;
        StartCoroutine(resetHurt());
    }
    IEnumerator resetHurt()
    {
        yield return new WaitForSeconds(0.15f);
        hurted = false;
    }



    public void CreateSlimeKingAtk(int num, Vector3 point, float radius)
    {

        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            var radians = 2 * MathF.PI / num * i;

            /* Get the vector direction */
            var vertical = MathF.Sin(radians);
            var horizontal = MathF.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var enemy = Instantiate(slimeKingAtk, spawnPos, Quaternion.identity) as GameObject;

            /* Rotate the enemy to face towards player */
            enemy.transform.LookAt(point);
            enemy.transform.eulerAngles = new Vector3(
            enemy.transform.eulerAngles.x + 25,
            enemy.transform.eulerAngles.y,
            enemy.transform.eulerAngles.z
            );
            /* Adjust height */
            enemy.transform.Translate(new Vector3(0, enemy.transform.localScale.y / 2, 0));
            enemy.GetComponent<Rigidbody>().AddForce(-enemy.transform.forward * 500);
            atking = false;
        }
    }
}
