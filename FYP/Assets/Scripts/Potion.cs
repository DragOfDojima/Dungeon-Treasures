using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static OVRPlugin;
using static UnityEngine.Rendering.DebugUI;

public class Potion : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] GameObject plug;
    [SerializeField] GameObject plugGrab;
    [SerializeField] MyGrabable mg;
    // Update is called once per frame
    bool done;
    void Update()
    {
        if (!done)
        {
            if (mg.getIsGrabing())
            {
                plugGrab.GetComponent<Grabbable>().enabled = true;
                plugGrab.GetComponent<GrabInteractable>().enabled = true;
                plugGrab.GetComponent<HandGrabInteractable>().enabled = true;

            }
            else
            {
                plugGrab.GetComponent<Grabbable>().enabled = false;
                plugGrab.GetComponent<GrabInteractable>().enabled = false;
                plugGrab.GetComponent<HandGrabInteractable>().enabled = false;

            }
            if (plugGrab.GetComponent<Grabbable>().SelectingPoints.Count > 0)
            {
                plug.GetComponent<Rigidbody>().isKinematic = false;
                plug.GetComponent<Rigidbody>().AddForce(transform.up * 10);
                done = true;
                Destroy(plugGrab);
                Destroy(plug, 3f);
                GetComponent<MyGrabable>().removeMatObject("plug");
            }
        }
        

    }



}
