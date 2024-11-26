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
        enemyRemain = mobspawner.getSpawnCount();

        text = "WAVE : " + WaveCounter + "   Enemy Remain : " + enemyRemain;
        textshow.text = text;

    }
}
