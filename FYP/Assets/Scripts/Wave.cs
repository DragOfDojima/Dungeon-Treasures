using Oculus.Interaction.HandGrab;
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
    bool rest = false;
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
        return waveCount;
    }
    // Update is called once per frame
    void Update()
    {
        if(mobspawner.getSpawnCount() == 0)
        {
            rest = true;
        }
        else
        {
            rest = false;
        }
        if (WaveMenu == null)
        {
            WaveMenu = GetComponent<StartMenuToCenter>().getStartMenu();
            mobspawner.setWaveMenu(GetComponent<StartMenuToCenter>().getStartMenu());
        }

        if(Input.GetKeyDown(KeyCode.B)) {
            NpcStat[] scripts = FindObjectsOfType<NpcStat>();
            foreach (NpcStat script in scripts)
            {
                script.kys();
            }
        }
    }

    public void resetWaveCount()
    {
        waveCount = 0;
        NpcStat[] scripts = FindObjectsOfType<NpcStat>();
        foreach (NpcStat script in scripts)
        {
            script.kys();
        }

        chest[] scripts2 = FindObjectsOfType<chest>();
        foreach (chest script in scripts2)
        {
            script.closeChest();
        }

        HandGrabInteractor[] scripts3 = FindObjectsOfType<HandGrabInteractor>();
        foreach (HandGrabInteractor script in scripts3)
        {
            script.Unselect();
        }

        CustomSocket[] scripts5 = FindObjectsOfType<CustomSocket>();
        foreach (CustomSocket script in scripts5)
        {
            script.Objectgrabed();
        }

        MyGrabable[] scripts4 = FindObjectsOfType<MyGrabable>();
        foreach (MyGrabable script in scripts4)
        {
            Destroy(script.gameObject);
        }

    }

    public bool isRest()
    {
        return rest;
    }
}
