using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class chest : MonoBehaviour
{
    Animator chestLid;
    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        chestLid = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            chestLid.Play("TreasureChest_OPEN",0,0.1f);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            chestLid.Play("TreasureChest_CLOSE", 0, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") { 
            Debug.Log("Open Chest");
            if (isOpen == false) { 
            chestLid.Play("TreasureChest_OPEN", 0, 0.1f);
            isOpen=true; 
            }
            
        else {
            chestLid.Play("TreasureChest_CLOSE", 0, 0.1f);
            isOpen=false;
        }
       }
   }
}

