using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MagicianAI : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private NpcStat npcStat;
    [SerializeField] private AudioClip deadSound;
    private int lastSkillIndex = -1;
    AudioSource audioSource;
    bool dead;
    bool startup;
    float Lasthp;

    public GameObject redOrb;
    public GameObject aoe;
    public GameObject slash;
    public GameObject fixedSpawnObject;
    // Start is called before the first frame update

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Lasthp = npcStat.getHP();
        StartCoroutine(AttackRoutine());

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
            if (Lasthp != npcStat.getHP())
            {
                hurt();
                Lasthp = npcStat.getHP();
            }
            Vector3 targetPosition = Camera.main.transform.position;
            FaceTarget(targetPosition);

        }
        else
        {
            if (!dead)
            {
                dead = true;
                gameObject.tag = "Untagged";
                animator.enabled = false;
                audioSource.clip = deadSound;
                animator.SetTrigger("dead");
                audioSource.Play();
            }
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    bool atk;

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            SelectSkill();
            yield return new WaitForSeconds(5f); // Wait for 5 seconds before the next attack
        }
    }
    private void SelectSkill()
    {
        Vector3 targetPosition = Camera.main.transform.position;

        int skillIndex;

        // Ensure the new skill is not the same as the last one
        do
        {
            skillIndex = Random.Range(0, 3); // Randomly select 0, 1, or 2
        } while (skillIndex == lastSkillIndex);

        // Reset all skill triggers (if necessary)
        animator.ResetTrigger("downToUp");
        animator.ResetTrigger("upToDown");
        animator.ResetTrigger("front");

        // Set the selected skill trigger
        switch (skillIndex)
        {
            case 0:
                animator.SetTrigger("downToUp");

                break;
            case 1:
                animator.SetTrigger("upToDown");

                break;
            case 2:
                animator.SetTrigger("front");

                break;
        }
        SpawnPrefab(skillIndex);
        // Store the last used skill index
        lastSkillIndex = skillIndex;
        
        if (!hurted)
        {

        }
    }

    private void SpawnPrefab(int index)
    {
        Vector3 spawnPosition;

        if (index == 2) // Assuming index 1 is for the AOE skill that spawns at a fixed position
        {
            spawnPosition = fixedSpawnObject.transform.position;
        }
        else
        {
            spawnPosition = Camera.main.transform.position; // Spawn at camera position for other skills
        }
        spawnPosition.y = 0;
        GameObject prefabToSpawn = null;

        // Select the correct prefab based on the index
        switch (index)
        {
            case 0:
                prefabToSpawn = redOrb;
                break;
            case 1:
                prefabToSpawn = aoe;
                break;
            case 2:
                prefabToSpawn = slash;
                break;
        }

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.LookRotation(transform.forward));
        }
        else
        {
            Debug.LogWarning("Invalid skill index: " + index);
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
    bool hurted;
    public void hurt()
    {
        hurted = true;
        animator.SetTrigger("damage");
        Invoke("resetHurt", 2);
    }
    void resetHurt()
    {
        hurted = false;
        animator.ResetTrigger("damage");
    }
}
