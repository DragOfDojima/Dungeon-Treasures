using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private GameObject midPointVisual,arrowPrefab,arrowSpawnPoint;
    [SerializeField]
    private float arrowMaxSpeed = 10;

    private GameObject owner;
    [SerializeField] private GameObject[] allPlayer;

    private void Start()
    {
        allPlayer = GameObject.FindGameObjectsWithTag("PlayerGO");
        owner = allPlayer[0];
    }
    public void PrepareArrow()
    {
        midPointVisual.SetActive(true);
    }
    [SerializeField]
    private AudioSource bowReleaseAudioSource;
    public void ReleaseArrow(float strength)
    {
        bowReleaseAudioSource.Play();
        midPointVisual.SetActive(false);
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = arrowSpawnPoint.transform.position;
        arrow.transform.rotation = midPointVisual.transform.rotation;
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(midPointVisual.transform.right*strength*arrowMaxSpeed,ForceMode.Impulse);
    }

    private void Update()
    {
        GameObject nearestPlayer = allPlayer[0];
        float distanceToNearest = Vector3.Distance(transform.position, nearestPlayer.transform.position);
        for (int i = 0; i < allPlayer.Length; i++)
        {
            float distanceToCurrent = Vector3.Distance(transform.position, allPlayer[i].transform.position);
            if (distanceToCurrent < distanceToNearest)
            {
                nearestPlayer = allPlayer[i];
                distanceToNearest = distanceToCurrent;
            }
        }
        owner = nearestPlayer;
    }
}
