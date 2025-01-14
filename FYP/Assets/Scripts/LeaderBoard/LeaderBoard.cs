using Oculus.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LeaderBoard",menuName ="Config/LeaderBoard")]
public class LeaderBoard : ScriptableObject
{
    public string sheetId;
    public string gridId;
    public List<leaderboard> lb;

    [ContextMenu("Sync")]
    public void Sync()
    {
        ReadGoogleSheets.FillData<leaderboard>(sheetId, gridId, list =>
        {
            lb= list;
            ReadGoogleSheets.SetDirty(this);
        });
    }
    [ContextMenu("OpenSheet")]
    private void Open()
    {
        ReadGoogleSheets.OpenUrl(sheetId,gridId);
    }
    [ContextMenu("Sort")]
    private void sortByScore()
    {
        lb.Sort((a, b) =>
        {
            int scoreComparison = b.Score.CompareTo(a.Score); // Descending score
            if (scoreComparison == 0)
            {
                return a.Time.CompareTo(b.Time); // Ascending time if scores are equal
            }
            return scoreComparison;
        });
    }
}

[Serializable]
public class leaderboard
{
    public string Name;
    public float Score;
    public float Time;
    public string Rate;
}
