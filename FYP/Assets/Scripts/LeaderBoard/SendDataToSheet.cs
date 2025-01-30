using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;
using UnityEngine.Networking;

public class SendDataToSheet : MonoBehaviour
{
    public string name;
    public string score;
    public string time;
    public string rate;

    public GameObject input;
    public GameObject main;
    public GameObject noInternet;
    public GameObject leaderboard;
    public GameObject inputName;

    private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfLh2I_Tug7Z0TayfYkmQReM56YtsH7c35IDDlzrrvudSIZGQ/formResponse";

    private List<string> inappropriateWords = new List<string>
    {
        "fuck", 
        "bitch",
        "dick",
        "idiot",
        "stupid",
        "dumb",
        "hate",
        "kill",
        "fool",
        "loser",
        "bitch",
        "asshole",
        "crap",
    };

    private Object floatText;

    private bool ContainsInappropriateWords(string name)
    {
        foreach (string word in inappropriateWords)
        {
            if (name.ToLower().Contains(word.ToLower())) // Case insensitive check
            {
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        floatText = Resources.Load("damageText");

    }
    public void SubmitFeedback()
    {
        name = input.GetComponent<TMP_InputField>().text;
        if (string.IsNullOrWhiteSpace(name)|| ContainsInappropriateWords(name))
        {
            var FloatText = Instantiate(floatText, transform.position, transform.rotation) as GameObject;
            FloatText.GetComponent<floattext>().setText("Inappropriate Name");
            FloatText.GetComponent<floattext>().setSize(0.95f);

            return;
        }
        StartCoroutine(Post(name, score, time, rate));
    }



    private IEnumerator Post(string data1, string data2, string data3, string data4)
    {
        if (!Application.internetReachability.Equals(NetworkReachability.NotReachable))
        {
            WWWForm form = new WWWForm();
            form.AddField("entry.1502373272", data1);
            form.AddField("entry.133371990", data2);
            form.AddField("entry.108790872", data3);
            form.AddField("entry.522198115", data4);

            using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Feedback success");
                }
                else
                {
                    Debug.Log("Error: " + www.error);
                }
            }
            leaderboard.SetActive(true);
            inputName.SetActive(false);
            leaderboard.GetComponent<UILeaderBoard>().getLeaderBoard();
        }
        else
        {
            Debug.Log("No internet connection. Please check your network settings.");
            HandleNoInternet();
        }
    }

    private void HandleNoInternet()
    {
        Debug.Log("Handling no internet connection...");
        noInternet.SetActive(true);
        main.SetActive(false);
    }


}
