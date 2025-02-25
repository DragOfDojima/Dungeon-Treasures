using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAi : MonoBehaviour
{
    public float smoothSpeed = 5f; // Speed of the smooth movement
    public LayerMask whatIsPlayer; // Layer for the player

    private Transform mainCamera; // Reference to the main camera's transform
    // Start is called before the first frame update

    void Start()
    {
        // Get the main camera's transform
        mainCamera = Camera.main.transform;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }
    // Update is called once per frame

    void Update()
    {
        bool playerInCloseRange = Physics.CheckSphere(transform.position, 1.2f, whatIsPlayer);
        Vector3 targetPosition = new Vector3(transform.position.x, mainCamera.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        if (!playerInCloseRange)
        {
            targetPosition = new Vector3(mainCamera.position.x, mainCamera.position.y, mainCamera.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        }
    }
    
}
