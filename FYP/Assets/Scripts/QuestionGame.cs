using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class QuestionGame : MonoBehaviour
{
    public TMP_Text questionText;
    public Button[] answerButtons;
    public GameObject Wrong_ans;
    public GameObject QuestionP;
    public UnlockTimer UL;
    public chest chest;



    private List<string[]> questionsAndAnswers;
    private int questionIndex;

    private void Update()
    {
        if (chest.QuestStart == true) { 
            questionIndex++;
            }

    }

    void Start()
    {
        LoadQuestionData();
        DisplayQuestion(questionIndex);

    }

    void LoadQuestionData()
    {
        string filePath = Application.dataPath + "/question_data.txt";
        questionsAndAnswers = new List<string[]>();

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');
                questionsAndAnswers.Add(data);
            }
        }
        else
        {
            Debug.LogError("Question data file not found!");
        }
    }

   
        void DisplayQuestion(int index)
    {
        if (questionsAndAnswers == null || index >= questionsAndAnswers.Count)
        {
            Debug.LogError("No more questions available!");
            return;
        }

        string[] questionData = questionsAndAnswers[index];

        questionText.text = questionData[0];

        // Shuffle the indices of the answer buttons
        int[] buttonIndices = new int[answerButtons.Length];
        for (int i = 0; i < buttonIndices.Length; i++)
        {
            buttonIndices[i] = i;
        }
        ShuffleArray(buttonIndices);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int buttonIndex = buttonIndices[i];
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = questionData[buttonIndex + 1];
        }
    }

    public void CheckAnswer(Button selectedButton)
    {
        string selectedAnswer = selectedButton.GetComponentInChildren<TMP_Text>().text;

        if (selectedAnswer == questionsAndAnswers[questionIndex][1])
        {
            DisplayQuestion(questionIndex);
            QuestionP.SetActive(false);
            chest.QuestStart = true;

        }
        else
        {   
            QuestionP.SetActive(false);
            Wrong_ans.SetActive(true);
            UL.Timer_on();

        }

    }

    void ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

}
