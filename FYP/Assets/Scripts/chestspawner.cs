using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestspawner : MonoBehaviour
{
    public float spawnTimer = 1;
    public GameObject prefabToSpawn;

    private float timer;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset;
    public int maxSpawn=5;
    void Start()
    {
        
    }
    int spawnCount=0;
    bool spawned;
    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            spawned=true;
            Spawn();
        }
        
    }

    public void Spawn()
    {
        GameObject[] chestSpawnerList = GameObject.FindGameObjectsWithTag("chestSpawn");
        for(int i =0; i < chestSpawnerList.Length; i++)
        {
            Instantiate(prefabToSpawn, chestSpawnerList[i].transform.position,Quaternion.identity);
        }
        /*MRUKRoom room =MRUK.Instance.GetCurrentRoom();
        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
        Vector3 randomPositionNormalOffset = pos + norm* normalOffset;
        randomPositionNormalOffset.y=0;
        Instantiate(prefabToSpawn, randomPositionNormalOffset, Quaternion.identity); */
    }

    public void killedMob()
    {
        spawnCount--;
    }
}
