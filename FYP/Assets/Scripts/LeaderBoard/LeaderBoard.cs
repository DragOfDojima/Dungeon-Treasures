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
    private void Sync()
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
}

[Serializable]
public class leaderboard
{
    public string Name;
    public float Score;
    public float Time;
    public string Rate;
}
