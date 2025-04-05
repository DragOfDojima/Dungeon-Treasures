using Photon.Pun;
using UnityEngine;
using TMPro;

public class NetworkedPlayer : MonoBehaviourPunCallbacks
{
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject nameTagObject;

    void Start()
    {
        if (photonView.IsMine)
        {
            // 啟用手部追蹤
            leftHand.SetActive(true);
            rightHand.SetActive(true);
            nameTagObject.SetActive(false);
        }
        else
        {
            // 停用手部追蹤，只當作模型顯示
            leftHand.SetActive(true);
            rightHand.SetActive(true);
            nameTagObject.SetActive(true);
            var tmp = nameTagObject.GetComponent<TextMeshPro>();
            tmp.text = photonView.Owner.NickName;
        }
    }

    void Update()
    {
        if (!photonView.IsMine && nameTagObject != null)
        {
            nameTagObject.transform.LookAt(Camera.main.transform);
        }
    }
}