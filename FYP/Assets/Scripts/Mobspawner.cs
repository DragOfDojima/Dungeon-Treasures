using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobspawner : MonoBehaviour
{
    public float spawnTimer = 1;
    public GameObject prefabToSpawn_slime;
    public GameObject prefabToSpawn_KingSlime;
    [SerializeField] GameObject WaveMenu;
    [SerializeField] GameObject WaveCounter;
    private float timer;
    [SerializeField] GameObject WIN;
    public Wave wave;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset;
    public int maxSpawn=3;
    private int SlimeCount;
    private int KingSlimeCount;
    //private int toBeSpawn;
    private int remain;
    void Start()
    {
    }

    public void setWaveMenu(GameObject w)
    {
        WaveMenu = w;
    }
    public void SetMobSpawn(int Slime, int kingSlime)
    {
        remain = Slime + kingSlime;
        SlimeCount = Slime;
        KingSlimeCount = kingSlime;
        //toBeSpawn = SlimeCount + KingSlimeCount;
        
    }

    int spawnCount=0;
    // Update is called once per frame
    void Update()
    {
        if (WaveMenu == null)
        {
            return;
        }

        if (remain <= 0)
        {
            StartCoroutine(wait());

        }
        else
        {
            WaveCounter.SetActive(true);
        }

        if (!MRUK.Instance&&!MRUK.Instance.IsInitialized)
            return;
        if(spawnCount >= maxSpawn)
            return;
        timer+=Time.deltaTime;
        if(timer > spawnTimer)
        {
            if(SlimeCount > 0) {
                Spawn(prefabToSpawn_slime);
                SlimeCount-=1;
                //toBeSpawn-=1;
            }
            else if(KingSlimeCount > 0) { 
                Spawn(prefabToSpawn_KingSlime);
                KingSlimeCount-=1;
                //toBeSpawn -= 1;
            }
            else if(!WaveMenu.activeSelf || remain==0){ 
                WaveClear();    
            }
            timer -= spawnTimer;
        }
    }

    public void Spawn(GameObject prefabToSpawn)
    {
        MRUKRoom room =MRUK.Instance.GetCurrentRoom();
        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
        Vector3 randomPositionNormalOffset = pos + norm* normalOffset;
        randomPositionNormalOffset.y=0;
        Instantiate(prefabToSpawn, randomPositionNormalOffset, Quaternion.identity); 
        spawnCount++;
    }

    public void killedMob()
    {
        spawnCount--;
        remain--;
    }

    public int getSpawnCount()
    {
        return remain; 
    }

    public void WaveClear()
    {
        WaveMenu.SetActive(true);
    }

    

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        if (remain <= 0)
        {
            WaveCounter.SetActive(false);
            if (wave.getWaveCount() == 3)
            {
                WIN.SetActive(true);
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(3);
                wave.resetWaveCount();

            }
        }
        
    }
}
