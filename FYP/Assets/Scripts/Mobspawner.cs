using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobspawner : MonoBehaviour
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
    // Update is called once per frame
    void Update()
    {
        if(!MRUK.Instance&&!MRUK.Instance.IsInitialized)
            return;
        if(spawnCount >= maxSpawn)
            return;
        timer+=Time.deltaTime;
        if(timer > spawnTimer)
        {
            Spawn();
            timer-=spawnTimer;
            spawnCount++;
        }
    }

    public void Spawn()
    {
        MRUKRoom room =MRUK.Instance.GetCurrentRoom();
        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
        Vector3 randomPositionNormalOffset = pos + norm* normalOffset;
        randomPositionNormalOffset.y=0;
        Instantiate(prefabToSpawn, randomPositionNormalOffset, Quaternion.identity); 
    }

    public void killedMob()
    {
        spawnCount--;
    }
}
