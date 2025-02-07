using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UILeaderBoard : MonoBehaviour
{

    public LeaderBoard leaderBoard;
    public GameObject[] lbs;
    public GameObject line;

    public GameObject mainLeaderBoard;
    public GameObject noInternet;
    public GameObject nointernetText;
    List<leaderboard> thisLeaderboard;

    string pName;
    string pTime;
    string pScore;
    string pRate;
    // Start is called before the first frame update
    void Awake()
    {
        setLB(leaderBoard.lb);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setLB(List<leaderboard> lb)
    {
        thisLeaderboard = lb;
    }

    public void setPdata(string n, string t, string s, string r)
    {
        pName = n;
        pTime = t;
        pScore = s;
        pRate = r;
    }
    void printLeaderBoard()
    {

        setLB(leaderBoard.lb);

        for (int i = 0; i < 10; i++)
        {
            if (i>leaderBoard.lb.Count-1)
            {
                lbs[i].SetActive(false);
                line.SetActive(false);
                lbs[10].SetActive(false);
            }
            else
            {
                Debug.Log("BUG"+ thisLeaderboard.Count);
                TextMeshProUGUI[] tmp = lbs[i].gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                tmp[1].text = thisLeaderboard[i].Name;
                tmp[2].text = thisLeaderboard[i].Time;
                tmp[3].text = thisLeaderboard[i].Score;
                tmp[4].text = thisLeaderboard[i].Rate;
            }
        }

        int j = FindPlayerPlacement(pName, pTime, pScore);
        if (j > 10)
        {
            line.SetActive(true);
            lbs[10].SetActive(true);
            TextMeshProUGUI[] tmp = lbs[10].gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            tmp[0].text = j.ToString();
            tmp[1].text = thisLeaderboard[j - 1].Name;
            tmp[2].text = thisLeaderboard[j - 1].Time;
            tmp[3].text = thisLeaderboard[j - 1].Score;
            tmp[4].text = thisLeaderboard[j - 1].Rate;
        }
        else
        {
            line.SetActive(false);
            lbs[10].SetActive(false);
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
            nointernetText.SetActive(true);
            Debug.Log("GetLeaderBoardError:" + e);
        }
    }

    public int FindPlayerPlacement(string name, string time, string score)
    {
        // Find the player's rank in the already sorted list
        for (int i = 0; i < thisLeaderboard.Count; i++)
        {
            if (thisLeaderboard[i].Name == name && thisLeaderboard[i].Time == time && thisLeaderboard[i].Score == score)
            {
                return i + 1; // Return 1-based rank
            }
        }

        return -1; // Player not found
    }
}
