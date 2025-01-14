using Meta.WitAi.Attributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendDataToSheet : MonoBehaviour
{
    public string data1;
    public string data2;
    public string data3;
    public string data4;

    private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfLh2I_Tug7Z0TayfYkmQReM56YtsH7c35IDDlzrrvudSIZGQ/formResponse\r\n";

    [Button]
    public void SubmitFeedback()
    {
        StartCoroutine(Post(data1,data2,data3,data4));
    }

    private IEnumerator Post(string data1, string data2, string data3, string data4)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1502373272", data1);
        form.AddField("entry.133371990", data2);
        form.AddField("entry.108790872", data3);
        form.AddField("entry.522198115", data4);

        using(UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                yield return www.SendWebRequest();

                if(www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Feedback successs");
                }
                else
                {
                    Debug.Log("Error "+www.error);
                }
            }
        }
    }
}
