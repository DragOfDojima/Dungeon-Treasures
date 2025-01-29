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
        sortByScore();
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
            // Parse scores from strings to integers for comparison
            int scoreA = int.TryParse(a.Score, out var parsedScoreA) ? parsedScoreA : 0;
            int scoreB = int.TryParse(b.Score, out var parsedScoreB) ? parsedScoreB : 0;

            // Compare scores in descending order
            int scoreComparison = scoreB.CompareTo(scoreA);

            // If scores are equal, compare times (parse the string to TimeSpan)
            if (scoreComparison == 0)
            {
                TimeSpan timeA = ParseTime(a.Time);
                TimeSpan timeB = ParseTime(b.Time);
                return timeA.CompareTo(timeB); // Ascending order for time
            }

            return scoreComparison; // Return the score comparison result
        });
    }
    private TimeSpan ParseTime(string time)
    {
        var parts = time.Split(':');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int minutes) &&
            int.TryParse(parts[1], out int seconds))
        {
            return new TimeSpan(0, minutes, seconds);
        }
        return TimeSpan.Zero; // Default value if parsing fails
    }
}

[Serializable]
public class leaderboard
{
    public string Name;
    public string Score;
    public string Time;
    public string Rate;
}
