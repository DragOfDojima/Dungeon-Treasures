using Meta.XR.MRUtilityKit;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Mobspawner : MonoBehaviourPun
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
    public int maxSpawn = 3;
    private int slimeCount;
    private int kingSlimeCount;
    private int remain;
    public bool spawnedKing;
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

    public IEnumerator SetMobSpawn(int slime, int kingSlime)
    {
        endWave = false;
        waitmob = true;
        WIN.SetActive(false);
        GetComponent<AudioSource>().Stop();
        WaveCounter.SetActive(true);
        remain = slime + kingSlime;
        yield return new WaitForSeconds(10f);
        slimeCount = slime;
        kingSlimeCount = kingSlime;
        started = true;
    }

    public bool getWaitmob()
    {
        return waitmob; // Ensure this method is present
    }

    int spawnCount = 0;

    void Update()
    {
        if (WaveMenu == null)
        {
            return;
        }

        if (remain <= 0 && !endWave && started)
        {
            endWave = true;
            StartCoroutine(wait());
        }

        if (!MRUK.Instance && !MRUK.Instance.IsInitialized)
            return;

        if (spawnCount >= maxSpawn)
            return;

        timer += Time.deltaTime;
        if (timer > spawnTimer)
        {
            if (slimeCount > 0)
            {
                Spawn(prefabToSpawn_slime);
                slimeCount -= 1;
            }
            else if (kingSlimeCount > 0)
            {
                Spawn(prefabToSpawn_KingSlime);
                spawnedKing = true;
                kingSlimeCount -= 1;
            }
            timer -= spawnTimer;
        }
    }

    public void Spawn(GameObject prefabToSpawn)
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            MRUKRoom room = MRUK.Instance.GetCurrentRoom();
            room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
            Vector3 randomPositionNormalOffset = pos + norm * normalOffset;
            randomPositionNormalOffset.y = 0;

            GameObject mob = PhotonNetwork.Instantiate(prefabToSpawn.name, randomPositionNormalOffset, Quaternion.identity);
            spawnCount++;
        }
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
        waitmob = false;
        started = false;
        WaveMenu.SetActive(true);
        chest[] scripts2 = FindObjectsOfType<chest>();
        foreach (chest script in scripts2)
        {
            script.closeChest();
        }
    }

    // WIN
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        WaveCounter.SetActive(false);
        if (wave.getWaveCount() == 3)
        {
            GameObject.FindGameObjectWithTag("PlayerGO").GetComponent<Player>().SpawnScoreBoard();
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