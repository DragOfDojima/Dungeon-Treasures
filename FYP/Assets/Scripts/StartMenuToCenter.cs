using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuToCenter : MonoBehaviour
{
    public GameObject canvasPrefab; // Reference to the 3D canvas prefab
    GameObject StartMenu;
    List<GameObject> wallObjects = new List<GameObject>();
    void Start()
    {

    }

    private void Update()
    {
        if (wallObjects.Count==0)
        {
            wallObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("chestSpawn"));

            // Ensure there are walls to calculate the center point
            if (wallObjects.Count == 0)
            {
                Debug.Log("No game objects with the tag 'Wall' found to calculate the center point.");
                return;
            }

            // Calculate the centroid of all wall objects
            Vector3 centroid = Vector3.zero;
            foreach (GameObject wallObject in wallObjects)
            {
                centroid += wallObject.transform.position;
            }
            centroid /= wallObjects.Count;
            Vector3 cameraPosition = Camera.main.transform.position;

            // Calculate the center point as the centroid of all walls
            Vector3 centerPoint = new Vector3(centroid.x, cameraPosition.y, centroid.z);

            Debug.Log("Center Point of the Room: " + centerPoint);

            // Create a 3D canvas at the center point
            StartMenu = Instantiate(canvasPrefab, centerPoint, Quaternion.identity);
            StartMenu.GetComponent<WaveCounter>().wave = GetComponent<Wave>();
        }
    }

    public GameObject getStartMenu()
    {
        return StartMenu;
    }
}
