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
    AudioSource audioSource;
    [SerializeField] AudioClip nonCombat;
    [SerializeField] AudioClip inCombat;
    [SerializeField] AudioClip boss;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void waveStart()
    {
        rest=false;
        WaveMenu.SetActive(false);
        switch (waveCount) { 
            case 0:
                StartCoroutine(mobspawner.SetMobSpawn(5, 0));
                waveCount = 1;
                break;
            case 1:
                StartCoroutine(mobspawner.SetMobSpawn(0, 1));
                waveCount = 2;
                break;
            case 2:
                StartCoroutine(mobspawner.SetMobSpawn(3, 1));
                waveCount = 3;
                break;
            default:
                Debug.Log("Invalid Wave");
                break;
        }

    }

    public int getWaveCount()
    {
        return waveCount;
    }
    // Update is called once per frame
    void Update()
    {
        if(mobspawner.getSpawnCount() <= 0&&!mobspawner.getWaitmob())
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

        UpdateAudioState();
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
    private void UpdateAudioState()
    {
        
  

        AudioClip newClip;

        if (rest)
        {
            newClip = nonCombat;
        }
        else if (mobspawner.spwanedking)
        {
            newClip = boss;
        }
        else
        {
            newClip = inCombat;
        }

        // Play the new clip if it's different from the current clip
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

    public bool isRest()
    {
        return rest;
    }
}
