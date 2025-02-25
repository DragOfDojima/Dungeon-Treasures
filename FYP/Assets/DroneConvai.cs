using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneConvai : MonoBehaviour
{
    public Transform player; // Assign the player object in the Inspector
    public float smoothSpeed = 5f; // Speed of the smooth movement

    private Transform mainCamera; // Reference to the main camera's transform

    void Start()
    {
        // Get the main camera's transform
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Make the object look at the player
            transform.LookAt(player);

            // Smoothly follow the Y position of the main camera
            
        }
    }


}
    
  
