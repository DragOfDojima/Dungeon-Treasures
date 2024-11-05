using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobspawner : MonoBehaviour
{
    public float spawnTimer = 1;
    public GameObject prefabToSpawn_slime;
    public GameObject prefabToSpawn_KingSlime;

    private float timer;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset;
    public int maxSpawn=5;
    [SerializeField] int SlimeCount;
    [SerializeField] int KingSlimeCount;
    void Start()
    {
        
    }

    public void SetMobSpawn(int Slime, int kingSlime)
    {
        SlimeCount = Slime;
        KingSlimeCount = kingSlime;
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
            if(SlimeCount > 0) {
                Spawn(prefabToSpawn_slime);
            }
            else if(KingSlimeCount > 0) { 
                Spawn(prefabToSpawn_KingSlime);
            }
        }
    }

    public void Spawn(GameObject prefabToSpawn)
    {
        MRUKRoom room =MRUK.Instance.GetCurrentRoom();
        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
        Vector3 randomPositionNormalOffset = pos + norm* normalOffset;
        randomPositionNormalOffset.y=0;
        Instantiate(prefabToSpawn, randomPositionNormalOffset, Quaternion.identity); 
        timer-=spawnTimer;
        spawnCount++;
    }

    public void killedMob()
    {
        spawnCount--;
    }
}
