using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    public Mobspawner mobspawner;
    private int waveCount = 0;
    GameObject WaveMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void waveStart()
    {
        switch (waveCount) { 
            case 0:
                mobspawner.SetMobSpawn(20, 0);
                waveCount = 1;
                break;
            case 1:
                mobspawner.SetMobSpawn(0, 1);
                waveCount = 2;
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
        if (WaveMenu == null)
        {
            WaveMenu = GetComponent<StartMenuToCenter>().getStartMenu();
        }
    }
}
