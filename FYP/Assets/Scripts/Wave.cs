using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    public Mobspawner mobspawner;
    private int waveCount = 1;
    [SerializeField] GameObject WaveMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void waveStart()
    {
        switch (waveCount) { 
            case 1:
                mobspawner.SetMobSpawn(20, 0);
                waveCount = 2;
                break;
            case 2:
                mobspawner.SetMobSpawn(0, 1);
                waveCount = 3;
                break;
            default:
                Debug.Log("Invalid Wave");
                break;
        }
        WaveMenu.SetActive(false);
    }

    public int getWaveCount()
    {
        return waveCount;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
