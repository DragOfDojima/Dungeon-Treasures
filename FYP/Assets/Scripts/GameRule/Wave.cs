using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    public Mobspawner mobspawner;
    public WhatEnemyToSpawn wets;
    private int waveCount = 0;
    GameObject WaveMenu;
    Button button;
    bool rest = true;
    private float timer;
    AudioSource audioSource;
    [SerializeField] AudioClip nonCombat;
    [SerializeField] AudioClip inCombat;
    [SerializeField] AudioClip boss;

    string dungeon;
    string questionFileName;

    bool chestSpawnOnGround;
    int chestCount;
    int potionChestCount;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void waveStart()
    {
        rest=false;
        WaveMenu.SetActive(false);
        wets.StartWave(waveCount);
        GameObject.Find("MobSpawner").GetComponent<Mobspawner>().GetWaveCounter().SetActive(true);
        waveCount++;
    }

    public int getWaveCount()
    {
        return waveCount;
    }
    // Update is called once per frame
    void Update()
    {
        /*(if(mobspawner.getSpawnCount() <= 0&&!mobspawner.getWaitmob())
        {
            rest = true;
        }
        else
        {
            rest = false;
        }*/
        if (WaveMenu == null)
        {
            WaveMenu = GetComponent<StartMenuToCenter>().getStartMenu();
            mobspawner.setWaveMenu(GetComponent<StartMenuToCenter>().getStartMenu());
        }
        if (!rest)
        {
            timer+=Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.B)) {
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
        timer = 0;
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
        WaveMenu.GetComponent<MainMenu>().Reset();
        gameObject.SetActive(false);

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

    public void setRest(bool r)
    {
        rest = r;
    }

    public float getTimer()
    {
        return timer;
    }

    public void setDungeon(string d)
    {
        dungeon = d;
        wets.setDungeon(d);
    }
    public void setQuestionFileName(string q)
    {
        questionFileName = q;
    }

    public string getQuestionFileName()
    {
        return questionFileName;
    }

    public void setGameRule(bool s,int c, int pc)
    {
        chestSpawnOnGround = s;
        chestCount = c;
        potionChestCount = pc;
    }

    public bool getChestSpawnOnGround()
    {
        return chestSpawnOnGround;
    }

    public int getChestCount()
    {
        return chestCount;
    }

    public int getPotionChestCount()
    {
        return potionChestCount;
    }
}
