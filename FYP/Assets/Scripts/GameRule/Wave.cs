using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Wave : MonoBehaviourPun
{
    public Mobspawner mobspawner;
    private int waveCount = 0;
    private GameObject WaveMenu;
    private bool rest = false;
    private float timer;
    private AudioSource audioSource;
    [SerializeField] private AudioClip nonCombat;
    [SerializeField] private AudioClip inCombat;
    [SerializeField] private AudioClip boss;

    private string dungeon;
    private string questionFileName;

    private bool chestSpawnOnGround;
    private int chestCount;
    private int potionChestCount;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void waveStart()
    {
        rest = false;
        WaveMenu.SetActive(false);
        switch (waveCount)
        {
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

    void Update()
    {
        if (mobspawner.getSpawnCount() <= 0 && !mobspawner.getWaitmob())
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
            mobspawner.setWaveMenu(WaveMenu);
        }

        if (!rest)
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
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
    }

    private void UpdateAudioState()
    {
        AudioClip newClip;

        if (rest)
        {
            newClip = nonCombat;
        }
        else if (mobspawner.spawnedKing) // Updated from spwaneding to spawnedKing
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

    public float getTimer()
    {
        return timer;
    }

    public void setDungeon(string d)
    {
        dungeon = d;
    }

    public void setQuestionFileName(string q)
    {
        questionFileName = q;
    }

    public string getQuestionFileName()
    {
        return questionFileName;
    }

    public void setGameRule(bool s, int c, int pc)
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

    [PunRPC]
    public void ResetWaveCountRPC()
    {
        resetWaveCount();
    }
}