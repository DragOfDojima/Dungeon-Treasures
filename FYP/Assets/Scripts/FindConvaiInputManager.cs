using Convai.Scripts.Runtime.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindConvaiInputManager : MonoBehaviour
{
    // Start is called before the first frame update

    ConvaiPlayerInteractionManager inputManager;
    public TMP_InputField inputField; 
    void Start()
    {
        inputManager = GameObject.Find("Convai NPC Helper").GetComponent<ConvaiPlayerInteractionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void send()
    {
        if(inputManager == null)
            inputManager = GameObject.Find("Convai NPC Helper").GetComponent<ConvaiPlayerInteractionManager>();

        inputManager.setInputField(inputField);
        inputManager.HandleSendText();
    }
}
