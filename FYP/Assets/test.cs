using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ServerStartWave();
        }
    }

    [ServerRpc]
    private void ServerStartWave()
    {
        Debug.Log("Server: Starting wave and notifying clients...");
        ClientStartWave();
    }

    [ClientRpc]
    private void ClientStartWave()
    {
        Debug.Log("Client: Received wave start notification.");
        // Your existing logic...
    }
}
