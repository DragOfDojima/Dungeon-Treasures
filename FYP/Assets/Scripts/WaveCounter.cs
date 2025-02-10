using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;


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
        ServerStartWave();
    }

    [ServerRpc]
    private void ServerStartWave()
    {
        ClientStartWave();
    }
    [ClientRpc]
    private void ClientStartWave()
    {
        if (GameObject.FindGameObjectsWithTag("ScoreBoard") != null)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("ScoreBoard"))
            {
                Destroy(g);
            }
        }
        Debug.Log("WORK-----------------------------------------------------------------------------------");
        wave.waveStart();
    }

    // Update is called once per frame
    void Update()
    {
        text = "WAVE "+(wave.getWaveCount()+1).ToString();
        textMeshPro.text = text;
    }
}
