using Meta.XR.MRUtilityKit;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class chestspawner : MonoBehaviourPun
{
    public float spawnTimer = 1;
    public GameObject normalChest;
    public GameObject potionChest;

    private float timer;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset;
    public int maxSpawn = 5;
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

    int spawnCount = 0;
    int potionSpawnCount = 0;
    bool spawned;
    bool rest;

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return; // Only the master client spawns chests

        if (scripts.Length == 0)
        {
            scripts = FindObjectsOfType<CheckChest>();
        }

        if (rest != wave.isRest() && !wave.isRest())
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
    }

    bool AllChestsHaveBeenTaken()
    {
        foreach (var script in scripts)
        {
            if (!script.getHaveChest())
            {
                return false;
            }
        }
        return true;
    }

    public void Spawn()
    {
        spawnCount = wave.getChestCount();
        potionSpawnCount = wave.getPotionChestCount();
        SpawnChestOnGround = wave.getChestSpawnOnGround();
        spawnNormalChest();
        spawnPotionChest();
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
        return UnityEngine.Random.value < 0.5f;
    }

    private void spawnNormalChest()
    {
        Quaternion parentRotationWithOffset;

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
                        PhotonNetwork.Instantiate(normalChest.name, randomPositionNormalOffset, Quaternion.identity);
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

                PhotonNetwork.Instantiate(normalChest.name, scripts[rand].transform.position, parentRotationWithOffset);
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
                        PhotonNetwork.Instantiate(potionChest.name, randomPositionNormalOffset, Quaternion.identity);
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

                PhotonNetwork.Instantiate(potionChest.name, scripts[rand].transform.position, parentRotationWithOffset);
                scripts[rand].setHaveChest(true);
                potionSpawnCount++;
            }
        }
    }
}