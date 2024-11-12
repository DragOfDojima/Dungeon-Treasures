using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandInteractor : XRDirectInteractor
{



    public void HandDetection(XRBaseInteractable interactable)
    {
        if(interactable is Arrow arrow)
        {
            arrow.sphereCollider.enabled = false;

        }

        if (interactable is Bow bow)
        {

        }


    }



}
