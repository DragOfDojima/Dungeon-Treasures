using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShowAllQustionSet : MonoBehaviour
{
    public GameObject layoutGroupObject;
    public GameObject qustionSet;
    public SearchFromGoogleSheet sfgs;
    private string filePath;
    List<List<string>> qustionsData;


    private void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "questionStat.txt");

        ClearAllContent();
        StartCoroutine(LoadQuestionStat());
        SpawnQuestionSets(qustionsData);
    }

    public void updateQuestions()
    {
        ClearAllContent();
        StartCoroutine(LoadQuestionStat());
    }
    public void ClearAllContent()
    {
        // Check if the layout group object is assigned
        if (layoutGroupObject != null)
        {
            // Get all child objects
            foreach (Transform child in layoutGroupObject.transform)
            {
                // Destroy each child object
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Layout group object is not assigned.");
        }
    }

    public void SpawnQuestionSets(List<List<string>> sheets)
    {

        foreach (Transform child in layoutGroupObject.transform)
        {
            Destroy(child.gameObject);
        }

        // Spawn a question set for each sheet
        foreach (var sheet in sheets)
        {
            if (sheet.Count >= 3) // Ensure there are at least three components
            {
                // Instantiate the question set prefab
                GameObject questionSet = Instantiate(qustionSet, layoutGroupObject.transform);
                QuestionsInfo questionsInfo = questionSet.GetComponent<QuestionsInfo>();
                questionsInfo.Setup(sheet[2], sheet[0], sheet[1],sfgs);
            }
            else
            {
                Debug.LogWarning("Sheet does not contain enough information.");
            }
        }
    }
    private IEnumerator LoadQuestionStat()
    {
        Debug.Log("File path: " + filePath);

//#if UNITY_EDITOR
        // In the Editor, read directly from StreamingAssets
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            Debug.Log("File content: " + data);
            qustionsData=ConvertStringToList(data);
        }
        else
        {
            Debug.LogError("Question data file not found in the Editor!");
        }
/*#else
        // On Android, use UnityWebRequest
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string data = request.downloadHandler.text;
                Debug.Log("File content: " + data);
                qustionsData=ConvertStringToList(data);
            }
            else
            {
                Debug.LogError("Error reading file: " + request.error);
            }
        }
#endif*/
        SpawnQuestionSets(qustionsData);
        yield return null;
    }

    public static List<List<string>> ConvertStringToList(string input)
    {
        // Split the input string into individual entries using new lines as the delimiter
        string[] entries = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Create a List to hold the rows
        List<List<string>> result = new List<List<string>>();

        // Populate the List
        foreach (string entry in entries)
        {
            // Split the entry into components by comma
            string[] components = entry.Split(',');

            // Ensure there are exactly three components
            if (components.Length == 3)
            {
                // Create a new List for the row and add components
                List<string> row = new List<string>
                {
                    components[0].Trim(), // sheetID
                    components[1].Trim(), // gridID
                    components[2].Trim()  // name
                };

                // Add the row to the result
                result.Add(row);
            }
            else
            {
                throw new FormatException("Each entry must contain exactly three components: sheetID, gridID, name.");
            }
        }

        return result;
    }

}
