using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILeaderBoard : MonoBehaviour
{

    public LeaderBoard leaderBoard;
    public GameObject[] lbs;
    public GameObject line;

    public GameObject mainLeaderBoard;
    public GameObject noInternet;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void printLeaderBoard()
    {
        for(int i = 0; i < 10; i++)
        {
            if (i>leaderBoard.lb.Count-1)
            {
                lbs[i].SetActive(false);
                line.SetActive(false);
                lbs[10].SetActive(false);
            }
            else
            {
                TextMeshProUGUI[] tmp = lbs[i].gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                tmp[1].text = leaderBoard.lb[i].Name;
                tmp[2].text = leaderBoard.lb[i].Time;
                tmp[3].text = leaderBoard.lb[i].Score;
                tmp[4].text = leaderBoard.lb[i].Rate;
            }
        }
        
    }

    public void getLeaderBoard()
    {
        try
        {
            leaderBoard.Sync();
            if (leaderBoard.lb.Count <= 0)
            {
                mainLeaderBoard.SetActive(false);
                noInternet.SetActive(true);
                return;
            }
            printLeaderBoard();
            mainLeaderBoard.SetActive(true);
            noInternet.SetActive(false);
        }
        catch (Exception e)
        {
            mainLeaderBoard.SetActive(false);
            noInternet.SetActive(true);
            Debug.Log("GetLeaderBoardError:" + e);
        }
    }
}
