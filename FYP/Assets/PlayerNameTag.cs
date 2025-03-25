using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameTag : MonoBehaviourPun
{
    public TextMeshProUGUI nameTag; // Reference to the TMP component

    void Start()
    {
        // If this is the local player, set the name tag to the player's name
        if (photonView.IsMine)
        {
            nameTag.text = PhotonNetwork.NickName; // Set the name tag to the player's name
        }
        else
        {
            nameTag.text = photonView.Owner.NickName; // Set name tag for other players
        }
    }
}