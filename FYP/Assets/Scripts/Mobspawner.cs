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
    public bool spwanedking;
    bool endWave;
    bool started;
    bool waitmob;
    void Start()
    {
    }

    public void setWaveMenu(GameObject w)
    {
        WaveMenu = w;
    }
    public IEnumerator SetMobSpawn(int Slime, int kingSlime)
    {
        endWave = false;
        waitmob=true;
        WIN.SetActive(false);
        GetComponent<AudioSource>().Stop();
        WaveCounter.SetActive(true);
        yield return new WaitForSeconds(10f);
        remain = Slime + kingSlime;
        SlimeCount = Slime;
        KingSlimeCount = kingSlime;
        started = true;
        //toBeSpawn = SlimeCount + KingSlimeCount;

    }
    public bool getWaitmob()
    {
        return waitmob;
    }
    int spawnCount=0;
    // Update is called once per frame
    void Update()
    {
        if (WaveMenu == null)
        {
            return;
        }

        if (remain <= 0&&!endWave&&started)
        {
            endWave = true;
            StartCoroutine(wait());
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
                spwanedking = true;
                KingSlimeCount-=1;
                //toBeSpawn -= 1;
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
        waitmob=false;
        started=false;
        WaveMenu.SetActive(true);
        chest[] scripts2 = FindObjectsOfType<chest>();
        foreach (chest script in scripts2)
        {
            script.closeChest();
        }
    }

    

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        WaveCounter.SetActive(false);
        if (wave.getWaveCount() == 3)
        {
            WIN.SetActive(true);
            GetComponent<AudioSource>().Play();
            StartCoroutine(endWin());
            yield return new WaitForSeconds(3);
            wave.resetWaveCount();
            
        }
        WaveClear();
    }

    IEnumerator endWin()
    {
        yield return new WaitForSeconds(26);
        WIN.SetActive(false);
    }
}
