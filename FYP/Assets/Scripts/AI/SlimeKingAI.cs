using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SlimeKingAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public Animator animator;
    public GameObject slimeKingAtk;
    public GameObject slimeKingAtkSpawner;
    [SerializeField]private NpcStat npcStat;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("jump", true);
        agent.updatePosition = false;
        Lasthp = npcStat.getHP();
    }

    // Update is called once per frame
    float Lasthp;
    bool atking=false;
    void Update()
    {
        if (!npcStat.getDead())
        {
            AtkPlayer();
        }
        else
        {
            gameObject.tag="Untagged";
            animator.enabled = false; 
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            CreateSlimeKingAtk(8, slimeKingAtkSpawner.transform.position, 1f);
        }
        
    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(1.1f);
        CreateSlimeKingAtk(8, slimeKingAtkSpawner.transform.position, 0.85f);
    }
    private void AtkPlayer()
    {
        Vector3 targetPosition = Camera.main.transform.position;
        //CreateSlimeKingAtk(8, slimeKingAtkSpawner.transform.position,2f);
        agent.SetDestination(targetPosition);
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slime_jumping_baked" && !atking)
        {
            atking = true;
            StartCoroutine(attack());
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
        
    }
    bool hurted;
    public void hurt()
    {
        hurted=true;
        Invoke("resetHurt",2);
    }
    void resetHurt()
    {
        hurted=false;
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
