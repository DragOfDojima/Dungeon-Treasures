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
    [SerializeField] GameObject liquid;
    private float capacity = 100;
    private bool ispouring;
    private bool isopened;
    // Update is called once per frame
    bool done;
    void Update()
    {
        if (!done)
        {
            if (mg.getIsGrabing()&& !isopened)
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
            if(!isopened)
            {
                if (plugGrab.GetComponent<Grabbable>().SelectingPoints.Count > 0)
                {
                    //plug.GetComponent<Rigidbody>().isKinematic = false;
                    //plug.GetComponent<Rigidbody>().AddForce(transform.up * 10);
                    done = true;
                    plug.GetComponent<Rigidbody>().isKinematic = false;
                    plug.transform.parent = null;
                    GetComponent<MyGrabable>().removeMatObject("plug");
                    isopened = true;
                }
            }
            if(isopened&&plugGrab!=null)
            {
                if (plugGrab.GetComponent<Grabbable>().SelectingPoints.Count == 0)
                {
                    Destroy(plugGrab);
                    Destroy(plug, 3f);
                }

            }
            
        }
        if(ispouring)
        {
            if(capacity > 0)
            capacity -= (48f * Time.deltaTime);
            
            capacity = Mathf.Max(capacity, 0);
            liquid.GetComponent<Renderer>().material.SetFloat("_fill", capacity/200);
        }

    }

    public void setIspouring(bool p)
    {
        ispouring = p;
    }

    public float Getcap()
    {
        return capacity;
    }

    public bool GetOpened()
    {
        return isopened;
    }

}
