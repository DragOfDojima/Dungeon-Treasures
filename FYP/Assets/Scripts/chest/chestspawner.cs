using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float ChestRespawnTime;
    CheckChest[] scripts;
    [SerializeField] Wave wave;
    void Start()
    {
        scripts = FindObjectsOfType<CheckChest>();
        
    }
    int spawnCount=0;
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

    public void Spawn()
    {
        foreach (CheckChest script in scripts)
        {
            Quaternion parentRotationWithOffset;
            if (!script.getHaveChest())
            {
                if (script.transform.parent.localScale.z > script.transform.parent.localScale.x * 2)
                {
                     parentRotationWithOffset = Quaternion.Euler(0, -90, 0) * script.transform.parent.rotation;
                }
                else
                {
                    parentRotationWithOffset = Quaternion.Euler(0, 0, 0) * script.transform.parent.rotation;
                }
                 
                Instantiate(prefabToSpawn, script.transform.position, parentRotationWithOffset);

            }
        }
        

        /*MRUKRoom room =MRUK.Instance.GetCurrentRoom();
        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
        Vector3 randomPositionNormalOffset = pos + norm* normalOffset;
        randomPositionNormalOffset.y=0;
        Instantiate(prefabToSpawn, randomPositionNormalOffset, Quaternion.identity); */
    }

}
