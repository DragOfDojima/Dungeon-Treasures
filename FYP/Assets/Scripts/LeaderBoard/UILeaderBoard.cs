using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILeaderBoard : MonoBehaviour
{

    public LeaderBoard leaderBoard;
    public GameObject[] lbs;
    // Start is called before the first frame update
    void Start()
    {
        leaderBoard.Sync();
        printLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void printLeaderBoard()
    {
        TextMeshProUGUI[] tmp = lbs[0].gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        tmp[1].text = leaderBoard.lb[0].Name;
        tmp[2].text = leaderBoard.lb[0].Time;
        tmp[3].text = leaderBoard.lb[0].Score;
        tmp[4].text = leaderBoard.lb[0].Rate;
    }
}
