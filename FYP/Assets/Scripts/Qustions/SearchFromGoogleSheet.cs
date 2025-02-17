using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class SearchFromGoogleSheet : MonoBehaviour
{
    public TMP_Text feedbackText;
    public GameObject Error;
    public GameObject Search;

    public TMP_InputField sheetIdInput; 
    public TMP_InputField gridIdInput;
    public TMP_InputField nameInput;

    public GameObject setName;
    public GameObject main;

    private Object floatText;

    public GameObject selectQuestion;
    public GameObject selectGameRule;
    public GameObject enterName;
    public GameObject typeAdd;


    [Serializable]
    public class QuizData
    {
        public string Question;
        public string Answer;
        public string FakeAnswer1;
        public string FakeAnswer2;
        public string FakeAnswer3;
    }
    public const char Determiner = ';';

    public string SheetId = "nothing"; // Replace with your Google Sheet ID
    public string GridId = "0"; // Replace with your Google Sheet Grid ID

    string Qdata;

    private void Start()
    {
        floatText = Resources.Load("damageText");
    }

    public void SubmitIds()
    {
        // Get the values from the input fields
        SheetId = sheetIdInput.text;
        GridId = gridIdInput.text;

        // You can call a function that uses these IDs here
        LoadWebClient(SheetId, GridId, s =>
        {
            string data = RemoveFirstLine(s);
            Debug.Log(data);
            UpdateFeedbackText(ValidateFormat(data));
        });
    }

    public void updateData(string sheetId, string gridId, string name)
    {
        string data="";
        LoadWebClient(sheetId, gridId, s =>
        {
            data = RemoveFirstLine(s);
            Debug.Log(data);
            if(ValidateFormat(data)== "OK: Format valid")
            {
                try
                {
                    string filePath;
                    filePath = Path.Combine(Application.streamingAssetsPath, name);
                    File.WriteAllText(filePath, data);
                }
                catch (IOException ex)
                {
                    Debug.LogError($"Failed to create file: {ex.Message}");
                }
            }
            else
            {
                var FloatText = Instantiate(floatText, transform.position, transform.rotation) as GameObject;
                FloatText.GetComponent<floattext>().setText("Google sheet format wrong");
                FloatText.GetComponent<floattext>().setSize(0.95f);
            }
        });

    }

    public void SubmitName()
    {
        string name = nameInput.text;
        string QStatPath= Path.Combine(Application.streamingAssetsPath, "questionStat.txt");
        if (!IsValidFileName(name))
        {
            var FloatText = Instantiate(floatText, transform.position, transform.rotation) as GameObject;
            FloatText.GetComponent<floattext>().setText("Name invalid");
            FloatText.GetComponent<floattext>().setSize(0.95f);
            return;
        }

        name = name + ".txt";
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, name);
        try
        {
            File.WriteAllText(filePath, Qdata);
            if (!DoesSheetIdExist(SheetId))
            File.AppendAllText(QStatPath,SheetId + "," + GridId + "," + name  + "\n");
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to create file: {ex.Message}");
        }
        main.SetActive(true);
        setName.SetActive(false);
        typeAdd.SetActive(false);
        gameObject.GetComponent<ShowAllQustionSet>().updateQuestions();
    }

    

    private bool IsValidFileName(string fileName)
    {
        // Check for invalid characters
        string invalidCharsPattern = @"[\\/:*?""<>|]";
        return !Regex.IsMatch(fileName, invalidCharsPattern) &&
               !fileName.Trim().EndsWith(".") &&
               !fileName.Trim().EndsWith(" ") &&
               fileName.Length > 0;
    }


    public string RemoveFirstLine(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input; // Return the original string if it's null or empty

        // Find the index of the first newline character
        int newlineIndex = input.IndexOf('\n');

        if (newlineIndex == -1)
            return string.Empty; // If there's no newline, return an empty string

        // Return the substring starting from the character after the first newline
        return input.Substring(newlineIndex + 1).TrimStart();
    }

    public static void LoadWebClient(string id, string gridId, Action<string> callBack)
    {
        string url = $@"https://docs.google.com/spreadsheet/ccc?key={id}&usp=sharing&output=csv&id=KEY&gid={gridId}";
        LoadWebClient3(url, callBack);
    }

    public static void LoadWebClient(string url, Action<string> callBack)
    {
        LoadWebClient3(url, callBack);
    }

    private static void LoadWebClient3(string id, Action<string> callBack)
    {
        WWW w = new WWW(id);
        while (!w.isDone)
            w.MoveNext();
        callBack(w.text);
    }

    public string ValidateFormat(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "ERR: Empty input";

        // Split the input into lines
        string[] lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Check if there are fewer than 5 lines
        if (lines.Length < 5)
            return "ERR: At least 5 lines required";

        foreach (string line in lines)
        {
            // Split the line by commas and trim whitespace
            string[] words = line.Split(',')
                                 .Select(word => word.Trim())
                                 .ToArray();

            // Check if there are fewer than 5 words
            if (words.Length < 5)
                return "ERR: Line must have at least 5 words";

            // Check if any word is null or empty
            if (words.Any(string.IsNullOrEmpty))
                return "ERR: Empty words found";
        }
        Qdata = input;
        return "OK: Format valid"; // All lines are valid
    }

    public void UpdateFeedbackText(string errorCode)
    {
        if (errorCode == "OK: Format valid")
        {
            enterName.SetActive(true);
            return;
        }
        Search.SetActive(false);
        Error.SetActive(true);
        switch (errorCode)
        {
            case "ERR: Empty input":
                feedbackText.text = "Please make sure you have set the google sheet to \"Anyone with the link\" can view, and connect to internet";
                break;
            case "ERR: At least 5 lines required":
                feedbackText.text = "Please make sure there is at least 5 questions.";
                break;
            case "ERR: Line must have at least 5 words":
                feedbackText.text = "Please make sure the google sheet is following the format.";
                break;
            case "ERR: Empty words found":
                feedbackText.text = "Please make sure each questions have no empty information.";
                break;
            default:
                feedbackText.text = "NO ERROR";
                break;
        }
    }

    public void afterSelectQuestion()
    {
        selectGameRule.SetActive(true);
        selectQuestion.SetActive(false);
    }

    public bool DoesSheetIdExist(string sheetID)
    {
        string QStatPath = Path.Combine(Application.streamingAssetsPath, "questionStat.txt");

        // Check if the file exists
        if (!File.Exists(QStatPath))
        {
            return false; // File does not exist, so the ID can't exist
        }

        // Read all lines from the file
        string[] lines = File.ReadAllLines(QStatPath);

        // Loop through each line to check for the sheetID
        foreach (string line in lines)
        {
            // Split the line by commas
            string[] parts = line.Split(',');

            // Check if the first part (sheetID) matches the given sheetID
            if (parts.Length > 0 && parts[0].Trim() == sheetID)
            {
                return true; // Found the sheetID
            }
        }

        return false; // sheetID not found
    }
}