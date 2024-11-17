using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private GameObject midPointVisual,arrowPrefab,arrowSpawnPoint;
    [SerializeField]
    private float arrowMaxSpeed = 10;

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
}
