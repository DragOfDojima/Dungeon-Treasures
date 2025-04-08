using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle toggle;
    public TextMeshProUGUI chest;
    public TextMeshProUGUI potionChest;
    public GameObject wavecounter;
    [SerializeField] GameObject WaveCount;
    [SerializeField] GameObject Menu;


    public void GameRuleDone()
    {
        GameObject.Find("GameM").GetComponent<Wave>().setGameRule(toggle.isOn, int.Parse(chest.text), int.Parse(potionChest.text));
    }

    public void setDungeon(string s)
    {
        GameObject.Find("GameM").GetComponent<Wave>().setDungeon(s);
    }

    public void Reset()
    {
        WaveCount.SetActive(false);
        Menu.SetActive(true);
    }
}
