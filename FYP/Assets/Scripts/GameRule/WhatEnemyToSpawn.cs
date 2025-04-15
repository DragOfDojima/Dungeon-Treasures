using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatEnemyToSpawn : MonoBehaviour
{
    public Mobspawner mobspawner;
    public GameObject protalRed;
    public GameObject protalBlue;
    public GameObject slime;
    public GameObject slimeKing;
    public GameObject knight;
    public GameObject magician;
    float normalOffset = 1;
    float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public string dungeon;
    List<List<GameObject>> waveMobCount = new List<List<GameObject>>();
    int curentMobCount;
    int maxMobOnMap=4;
    bool isFighting=false;
    bool isSpawning = false;
    int remainMob=0;

    private void Update()
    {
        curentMobCount = GameObject.FindGameObjectsWithTag("hitable").Length;
        if (isFighting && curentMobCount == 0&&!isSpawning)
        {
            mobspawner.endedWave(waveMobCount.Count);
            isFighting=false;
        }
    }
    public void setDungeon(string d)
    {
        waveMobCount.Clear();
        List<GameObject> Wave1;
        List<GameObject> Wave2;
        List<GameObject> Wave3;
        List<GameObject> Wave4;
        List<GameObject> Wave5;
        List<GameObject> Wave6;
        List<GameObject> Wave7;
        List<GameObject> Wave8;
        List<GameObject> Wave9;

        dungeon = d;
        switch (dungeon)
        {
            case "slime":
                Wave1 = new List<GameObject> {slime,slime,slime};
                Wave2 = new List<GameObject> {slime,slime,slime,slime,slime,slime,slime};
                Wave3 = new List<GameObject> {slimeKing};
                waveMobCount.Add(Wave1);
                waveMobCount.Add(Wave2);
                waveMobCount.Add(Wave3);
                break;
            case "knight":
                Wave1 = new List<GameObject> { knight };
                Wave2 = new List<GameObject> { knight, knight, knight };
                Wave3 = new List<GameObject> { knight, knight, knight, knight, knight };
                Wave4 = new List<GameObject> { magician, knight, knight};
                waveMobCount.Add(Wave1);
                waveMobCount.Add(Wave2);
                waveMobCount.Add(Wave3);
                waveMobCount.Add(Wave4);

                break;
            case "challenge":
                Wave1 = new List<GameObject> { slime, slime, slime };
                Wave2 = new List<GameObject> { knight, knight, knight };
                Wave3 = new List<GameObject> { slime, slime, slime, knight, knight };
                Wave4 = new List<GameObject> { slime, slime, slime, slime, slime, slime, slime,slime,slime,slime };
                Wave5 = new List<GameObject> { slimeKing ,slime,slime};
                Wave6 = new List<GameObject> { knight,knight,knight,slime,slime};
                Wave7 = new List<GameObject> { slimeKing, slimeKing};
                Wave8 = new List<GameObject> { magician , magician };
                Wave9 = new List<GameObject> { magician, slimeKing };
                waveMobCount.Add(Wave1);
                waveMobCount.Add(Wave2);
                waveMobCount.Add(Wave3);
                waveMobCount.Add(Wave4);
                waveMobCount.Add(Wave5);
                waveMobCount.Add(Wave6);
                waveMobCount.Add(Wave7);
                waveMobCount.Add(Wave8);
                waveMobCount.Add(Wave9);

                break;

        }
    }

    public void StartWave(int waveCount)
    {
        remainMob = waveMobCount[waveCount].Count;
        StartCoroutine(SpawnWave(waveCount));
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        isSpawning=true;
        curentMobCount = GameObject.FindGameObjectsWithTag("hitable").Length;
        for (int i = 0; i < waveMobCount[waveCount].Count; i++)
        {
            curentMobCount = GameObject.FindGameObjectsWithTag("hitable").Length;

            // Wait until currentMobCount is less than maxMobOnMap
            while (curentMobCount >= maxMobOnMap)
            {
                yield return null; // Wait until the next frame
                curentMobCount = GameObject.FindGameObjectsWithTag("hitable").Length; // Update current mob count
            }

            // Spawn the enemy
            if (waveMobCount[waveCount][i] == slimeKing || waveMobCount[waveCount][i] == magician)
            {
                Spawn(protalRed, waveMobCount[waveCount][i]);
            }
            else
            {
                Spawn(protalBlue, waveMobCount[waveCount][i]);
            }
            // Optionally, you can add a small delay between spawns
            yield return new WaitForSeconds(5f); // Adjust the delay as needed
        }
        isSpawning=false;
    }

    public void Spawn(GameObject protal, GameObject prefabToSpawn)
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
        Vector3 randomPositionNormalOffset = pos + norm * normalOffset;
        randomPositionNormalOffset.y = 0;
        var protalgo = Instantiate(protal, randomPositionNormalOffset, Quaternion.identity);
        protalgo.GetComponent<PortalSpwanEnemy>().go=prefabToSpawn;
        isFighting = true;
        Invoke("setIsFighting",3f);
    }

    public void setIsFighting()
    {
        isFighting=true;
    }

    public int getRemainMobCount()
    {
        return remainMob;
    }

    public void killedMob()
    {
        remainMob--;
    }
}
