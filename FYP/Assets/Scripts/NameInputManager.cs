using UnityEngine;
using UnityEngine.UI; // Required for Button
using TMPro; // Required for TMP_InputField
using Photon.Pun; // Required for Photon
using Photon.Realtime; // Required for Player

public class NameInputManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nameInputField; // TextMeshPro Input Field
    public Button submitButton; // Button for submitting the name
    public GameObject uiPanel; // Panel containing the input field and button
    public GameObject playerPrefab; // Prefab for the player
    public GameObject masterClientIndicator; // UI element to indicate master client

    private void Start()
    {
        // Hide UI initially
        uiPanel.SetActive(false);

        // Connect to Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ShowNameInputUI()
    {
        uiPanel.SetActive(true);
        submitButton.onClick.RemoveAllListeners(); // Clear previous listeners
        submitButton.onClick.AddListener(OnSubmitName);
    }

    void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim(); // Get text from TMP_InputField and trim whitespace
        if (!string.IsNullOrEmpty(playerName)) // Check if the name is valid
        {
            PhotonNetwork.NickName = playerName; // Set the player's name
            uiPanel.SetActive(false); // Hide the name input UI

            // Create or join the room
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ShowNameInputUI(); // Show UI when connected
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 }); // Create a room
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0); // Instantiate player
        UpdateMasterClientIndicator(); // Update the master client indicator
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateMasterClientIndicator(); // Update the indicator when a player joins
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdateMasterClientIndicator(); // Update the indicator when a player leaves
    }

    private void UpdateMasterClientIndicator()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            masterClientIndicator.SetActive(true); // Show indicator for master client
        }
        else
        {
            masterClientIndicator.SetActive(false); // Hide for non-master clients
        }
    }
}