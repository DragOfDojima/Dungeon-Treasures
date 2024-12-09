using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveCounter : MonoBehaviour
{
    public Wave wave;
    [SerializeField] TextMeshProUGUI textMeshPro;
    private string text;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameM = GameObject.Find("GameM");
        wave = gameM.GetComponent<Wave>();
    }

    public void StartWave()
    {
        wave.waveStart();
    }
    // Update is called once per frame
    void Update()
    {
        text = "WAVE "+(wave.getWaveCount()+1).ToString();
        textMeshPro.text = text;
    }
}
