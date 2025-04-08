using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class chestspawner : MonoBehaviour
{
    public float spawnTimer = 1;
    public GameObject normalChest;
    public GameObject potionChest;

    private float timer;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset;
    public int maxSpawn=5;
    public float ChestRespawnTime;
    CheckChest[] scripts;
    [SerializeField] Wave wave;
    [SerializeField] int maxChest;
    [SerializeField] int maxPotionChest;
    [SerializeField] bool SpawnChestOnGround;
    void Start()
    {
        scripts = FindObjectsOfType<CheckChest>();
        
    }
    int spawnCount=0;
    int potionSpawnCount = 0;
    bool spawned;
    bool rest;
    // Update is called once per frame
    void Update()
    {
        if (scripts.Length == 0)
        {
            scripts = FindObjectsOfType<CheckChest>();
        }
        if (rest!= wave.isRest()&&!wave.isRest())
        {
            foreach (CheckChest script in scripts)
            {
                script.StopAllCoroutines();
                script.setHaveChest(false);
            }
        }
        rest = wave.isRest();
        if (!wave.isRest())
        {
            Spawn();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            chest[] scripts = FindObjectsOfType<chest>();
            foreach (chest script in scripts)
            {
                script.closeChest();
            }
        }

    }

    bool AllChestsHaveBeenTaken()
    {
        foreach (var script in scripts)
        {
            if (!script.getHaveChest())
            {
                return false; // Return false if any script does not have a chest
            }
        }
        return true; // All scripts have a chest
    }
    public void Spawn()
    {
        maxChest = wave.getChestCount();
        maxPotionChest = wave.getPotionChestCount();
        SpawnChestOnGround = wave.getChestSpawnOnGround();
        spawnNormalChest();
        spawnPotionChest();
        /*MRUKRoom room =MRUK.Instance.GetCurrentRoom();
        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
        Vector3 randomPositionNormalOffset = pos + norm* normalOffset;
        randomPositionNormalOffset.y=0;
        Instantiate(prefabToSpawn, randomPositionNormalOffset, Quaternion.identity); */
    }
    public void chestClose()
    {
        spawnCount--;
    }
    public void potionChestClose()
    {
        potionSpawnCount--;
    }

    bool halfChance()
    {
        return UnityEngine.Random.value < 0.5f; // Returns true if successful
    }

    private void spawnNormalChest()
    {
        Quaternion parentRotationWithOffset;
        //Spawn Chest On table 
        for (int i = spawnCount; i < maxChest; i++)
        {
            if (SpawnChestOnGround)
            {
                if (halfChance())
                {
                    MRUKRoom room = MRUK.Instance.GetCurrentRoom();
                    room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
                    Vector3 randomPositionNormalOffset = pos + norm * normalOffset;
                    randomPositionNormalOffset.y = 0;
                    if (spawnCount < maxChest)
                    {
                        Instantiate(normalChest, randomPositionNormalOffset, Quaternion.identity);
                        spawnCount++;
                        return;
                    }
                }
            }
                
            int rand = UnityEngine.Random.Range(0, scripts.Length);
            if (AllChestsHaveBeenTaken()) return;
            while (scripts[rand].getHaveChest())
            {
                rand = UnityEngine.Random.Range(0, scripts.Length);
            }

            if (!scripts[rand].getHaveChest())
            {
                if (scripts[rand].transform.parent.localScale.z > scripts[rand].transform.parent.localScale.x * 2)
                {
                    parentRotationWithOffset = Quaternion.Euler(0, -90, 0) * scripts[rand].transform.parent.rotation;
                }
                else
                {
                    parentRotationWithOffset = Quaternion.Euler(0, 0, 0) * scripts[rand].transform.parent.rotation;
                }

                Instantiate(normalChest, scripts[rand].transform.position, parentRotationWithOffset);
                scripts[rand].setHaveChest(true);
                spawnCount++;
            }
        }
        
    }
    private void spawnPotionChest()
    {
        Quaternion parentRotationWithOffset;

        for (int i = potionSpawnCount; i < maxPotionChest; i++)
        {
            if (SpawnChestOnGround)
            {
                if (halfChance())
                {
                    MRUKRoom room = MRUK.Instance.GetCurrentRoom();
                    room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
                    Vector3 randomPositionNormalOffset = pos + norm * normalOffset;
                    randomPositionNormalOffset.y = 0;
                    if (potionSpawnCount < maxPotionChest)
                    {
                        Instantiate(potionChest, randomPositionNormalOffset, Quaternion.identity);
                        potionSpawnCount++;
                        return;
                    }
                }
            }
            int rand = UnityEngine.Random.Range(0, scripts.Length);
            if (AllChestsHaveBeenTaken()) return;
            while (scripts[rand].getHaveChest())
            {
                rand = UnityEngine.Random.Range(0, scripts.Length);
            }

            if (!scripts[rand].getHaveChest())
            {
                if (scripts[rand].transform.parent.localScale.z > scripts[rand].transform.parent.localScale.x * 2)
                {
                    parentRotationWithOffset = Quaternion.Euler(0, -90, 0) * scripts[rand].transform.parent.rotation;
                }
                else
                {
                    parentRotationWithOffset = Quaternion.Euler(0, 0, 0) * scripts[rand].transform.parent.rotation;
                }
                Instantiate(potionChest, scripts[rand].transform.position, parentRotationWithOffset);
                scripts[rand].setHaveChest(true);
                potionSpawnCount++;
            }
        }
    }
}
