using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

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

    AudioSource audioSource;
    [SerializeField] AudioClip correct;
    [SerializeField] AudioClip wrong;
    private void Update()
    {
        

    }
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "question_data.txt");
        RequestPermissions();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LoadQuestionData());
        DisplayQuestion();

    }

    private IEnumerator LoadQuestionData()
    {
        Debug.Log("File path: " + filePath);

#if UNITY_EDITOR
        // In the Editor, read directly from StreamingAssets
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            Debug.Log("File content: " + data);
            ParseData(data);
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
                ParseData(data);
            }
            else
            {
                Debug.LogError("Error reading file: " + request.error);
            }
        }
#endif

        // Call DisplayQuestion after loading data
        DisplayQuestion(); // Adjust the index as needed
        yield return null;
    }

    private void ParseData(string data)
    {
        questionsAndAnswers = new List<string[]>();
        string[] lines = data.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] questionData = lines[i].Split(',');
            questionsAndAnswers.Add(questionData);
        }
    }


    void DisplayQuestion()
    {
        int index;
        int randomIndex = Random.Range(0, questionsAndAnswers.Count);
        questionIndex = randomIndex;
        index = randomIndex;
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
            DisplayQuestion();
            audioSource.clip = correct;
            audioSource.Play();
            QuestionP.SetActive(false);
            chest.QuestStart = true;

        }
        else
        {
            audioSource.clip = wrong;
            audioSource.Play();
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

    private void RequestPermissions()
    {
#if UNITY_ANDROID
        if (!HasPermissions())
        {
            // Request necessary permissions
            UnityEngine.Android.Permission.RequestUserPermission("android.permission.READ_EXTERNAL_STORAGE");
            UnityEngine.Android.Permission.RequestUserPermission("android.permission.WRITE_EXTERNAL_STORAGE");
        }
#endif
    }

    private bool HasPermissions()
    {
#if UNITY_ANDROID
        return UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE") &&
               UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE");
#else
        return true; // For other platforms, assume permission is granted
#endif
    }

}
