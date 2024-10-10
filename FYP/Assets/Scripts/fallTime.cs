using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallTime : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawndelay",3f);
    }

    // Update is called once per frame
    public void Spawndelay()
    {
        cube.SetActive(true); 
    }
}
