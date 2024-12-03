using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    public Mobspawner mobspawner;
    private int waveCount = 0;
    GameObject WaveMenu;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void waveStart()
    {
        switch (waveCount) { 
            case 0:
                mobspawner.SetMobSpawn(5, 0);
                waveCount = 1;
                break;
            case 1:
                mobspawner.SetMobSpawn(5, 1);
                waveCount = 2;
                break;
            case 2:
                mobspawner.SetMobSpawn(0, 2);
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
        return waveCount+1;
    }
    // Update is called once per frame
    void Update()
    {
        if (WaveMenu == null)
        {
            WaveMenu = GetComponent<StartMenuToCenter>().getStartMenu();
            mobspawner.setWaveMenu(GetComponent<StartMenuToCenter>().getStartMenu());
        }
    }

    public void resetWaveCount()
    {
        waveCount=0;

    }
}
