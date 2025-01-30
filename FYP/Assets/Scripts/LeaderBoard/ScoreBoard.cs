using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public SendDataToSheet sdts;
    private int Score;
    private float TotalDamage;
    private int TotalEnemySlayed;
    private int TotalAnswerCorrect;
    private string Time;

    public GameObject totalSlay;
    public GameObject totalDamage;
    public GameObject correctAmswer;
    public GameObject time;
    public GameObject totalScore;
    public GameObject rank;
    public void setData(int s, float td, int tes, int tac,string t)
    {
        Score = s;
        TotalDamage = td;
        TotalEnemySlayed = tes;
        TotalAnswerCorrect = tac;
        Time = t;
        totalSlay.GetComponent<TextMeshProUGUI>().text = TotalEnemySlayed.ToString();
        totalDamage.GetComponent<TextMeshProUGUI>().text = TotalDamage.ToString();
        correctAmswer.GetComponent<TextMeshProUGUI>().text = TotalAnswerCorrect.ToString();
        time.GetComponent<TextMeshProUGUI>().text = Time;
        Score = Score + (int)Mathf.Floor(TotalDamage) + TotalAnswerCorrect * 50;

        string[] timeParts = Time.Split(':');
        int minutes = int.Parse(timeParts[0]);

        // Update the score based on minutes only
        if (minutes < 5) // Less than 5 minutes
        {
            Score += 1000;
        }
        else if (minutes < 10) // Less than 10 minutes
        {
            Score += 500;
        }
        totalScore.GetComponent<TextMeshProUGUI>().text = Score.ToString();
        rank.GetComponent<TextMeshProUGUI>().text = CalculateRating(Score,2300);

    }

    string CalculateRating(int score, int maxScore)
    {
        float percentage = (float)score / maxScore;

        if (percentage >= 0.9f) // 90% and above
        {
            return "S";
        }
        else if (percentage >= 0.8f) // 80% to 89%
        {
            return "A";
        }
        else if (percentage >= 0.7f) // 70% to 79%
        {
            return "B";
        }
        else if (percentage >= 0.6f) // 60% to 69%
        {
            return "C";
        }
        else if (percentage >= 0.5f) // 50% to 59%
        {
            return "D";
        }
        else if (percentage >= 0.4f) // 40% to 49%
        {
            return "E";
        }
        else // Below 40%
        {
            return "F";
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
