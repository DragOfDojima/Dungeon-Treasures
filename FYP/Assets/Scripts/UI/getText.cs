using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getText : MonoBehaviour
{
    [SerializeField] Wave wave;
    [SerializeField] Mobspawner mobspawner; 
    int WaveCounter;
    int enemyRemain;
    float timer;
    string time;
    string text;
    [SerializeField] Text textshow;

    // Start is called before the first frame update
    void Start()
    {
       
    }



    // Update is called once per frame
    void Update()
    {

        WaveCounter = wave.getWaveCount();
        enemyRemain = wave.GetWhatEnemyToSpawn().getRemainMobCount();
        timer = wave.getTimer();

        float minutes = Mathf.FloorToInt(timer / 60);
        if (minutes > 99) { minutes=99; }
        float seconds = Mathf.FloorToInt(timer % 60);

        text = "WAVE : " + WaveCounter + "   Enemy Remain : " + enemyRemain + "   Timer : " + string.Format("{0:00}:{1:00}", minutes, seconds);
        time = string.Format("{0:00}:{1:00}", minutes, seconds);
        textshow.text = text;
        //Debug.Log(text);
    }

    public string getTime()
    {
        return time;
    }
}
