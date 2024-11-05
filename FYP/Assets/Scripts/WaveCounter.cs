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
        //textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text = "WAVE "+wave.getWaveCount().ToString();
        textMeshPro.text = text;
    }
}
