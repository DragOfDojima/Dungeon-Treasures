using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestionsInfo : MonoBehaviour
{
    private string name;
    private string sheetID;
    private string gridID;
    private SearchFromGoogleSheet sfgs;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI content;

    public void Setup(string name, string sheetID, string gridID, SearchFromGoogleSheet s)
    {
        SetName(name);
        SetSheetID(sheetID);
        SetGridID(gridID);
        sfgs = s;
        nameText.text = name.Substring(0, name.Length - 4); ;
        string filePath = Path.Combine(Application.streamingAssetsPath, name);
        StartCoroutine(LoadData(filePath));
    }

    private IEnumerator LoadData(string filePath)
    {
        Debug.Log("File path: " + filePath);

#if UNITY_EDITOR
        // In the Editor, read directly from StreamingAssets
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            Debug.Log("File content: " + data);
            content.text = data;
        }
        else
        {
            Debug.LogError("Question data file not found in the Editor!");
        }
#else
       // On Android, use UnityWebRequest
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string data = request.downloadHandler.text;
                Debug.Log("File content: " + data);
                content.text = data;
            }
            else
            {
                Debug.LogError("Error reading file: " + request.error);
            }
        }
#endif
        yield return null;
    }

    public void updateQuestion()
    {
        sfgs.updateData(sheetID, gridID, name);
        string filePath = Path.Combine(Application.streamingAssetsPath, name);
        StartCoroutine(LoadData(filePath));
    }

    public void select()
    {
        GameObject.Find("GameM").GetComponent<Wave>().setQuestionFileName(name);
        sfgs.afterSelectQuestion();
    }
    public string GetName()
    {
        return name;
    }

    public void SetName(string value)
    {
        name = value;
    }

    public string GetSheetID()
    {
        return sheetID;
    }

    public void SetSheetID(string value)
    {
        sheetID = value;
    }

    public string GetGridID()
    {
        return gridID;
    }

    public void SetGridID(string value)
    {
        gridID = value;
    }
    
}
