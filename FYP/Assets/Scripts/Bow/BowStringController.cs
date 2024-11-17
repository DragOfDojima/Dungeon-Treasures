using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using Oculus.Interaction;
using static OVRPlugin;
using UnityEngine.Events;
using System;

public class BowStringController : MonoBehaviour
{
    [SerializeField]
    private BowString bowStringRenderer;

    private Grabbable interactable;

    [SerializeField]
    private Transform midPointGrabObject, midPointVisualObject, midPointParent;

    [SerializeField]
    private float bowStringStretchLimit;

    private Transform interactor;
    bool isgrabing;

    private float strength, previousStrength;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float stringSoundThreshold = 0.001f;

    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;
    private void Awake()
    {
        interactable = midPointGrabObject.GetComponent<Grabbable>();
    }

    private void Start()
    {
       // interactable.selectEntered.AddListener(PrepareBowString);
        //interactable.selectExited.AddListener(ResetBowString);
    }

    private void ResetBowString()
    {
        OnBowReleased?.Invoke(strength);
        strength = 0;
        previousStrength = 0;
        audioSource.pitch = 1;
        audioSource.Stop();

        interactor = null;
        midPointGrabObject.localPosition = Vector3.zero;
        midPointVisualObject.localPosition = Vector3.zero;
        bowStringRenderer.CreateString(null);
        
    }

    private void PrepareBowString()
    {
        //interactor = arg0.interactorObject.transform;
        interactor = midPointGrabObject.transform;
        OnBowPulled?.Invoke();
    }
    bool getIsgrabing()
    {
        return isgrabing;
    }
    private void Update()
    {
        if (interactable.SelectingPoints.Count > 0)
        {
            isgrabing = true;
            PrepareBowString();
        }
        if (isgrabing&& !(interactable.SelectingPoints.Count > 0))
        {
            isgrabing = false;
            ResetBowString();
        }
        
        if (interactor != null)
        {
            //convert bow string mid point position to the local space of the MidPoint
            Vector3 midPointLocalSpace =
                midPointParent.InverseTransformPoint(midPointGrabObject.position); // localPosition

            //get the offset
            float midPointLocalZAbs = Mathf.Abs(midPointLocalSpace.x);

            previousStrength = strength;

            HandleStringPushedBackToStart(midPointLocalSpace);

            HandleStringPulledBackTolimit(midPointLocalZAbs, midPointLocalSpace);

            HandlePullingString(midPointLocalZAbs, midPointLocalSpace);

            bowStringRenderer.CreateString(midPointVisualObject.position);
        }
    }

    private void HandlePullingString(float midPointLocalZAbs, Vector3 midPointLocalSpace)
    {
        //what happens when we are between point 0 and the string pull limit
        if (midPointLocalSpace.x < 0 && midPointLocalZAbs < bowStringStretchLimit)
        {

            if (audioSource.isPlaying == false && strength <= 0.01f)
            {
                audioSource.Play();
            }
            strength = Remap(midPointLocalZAbs, 0,bowStringStretchLimit,0,1); 
            midPointVisualObject.localPosition = new Vector3(midPointLocalSpace.x, 0, 0);
            PlayStringPullinSound();
        }
    }

    private void PlayStringPullinSound()
    {
        //Check if we have moved the string enought to play the sound unpause it
        if (Mathf.Abs(strength - previousStrength) > stringSoundThreshold)
        {
            if (strength < previousStrength)
            {
                //Play string sound in reverse if we are pusing the string towards the bow
                audioSource.pitch = -1;
            }
            else
            {
                //Play the sound normally
                audioSource.pitch = 1;
            }
            audioSource.UnPause();
        }
        else
        {
            //if we stop moving Pause the sounds
            audioSource.Pause();
        }

    }

    private float Remap(float value, int fromMin,float fromMax, int toMin , int toMax )
    {
        return (value - fromMin) / (fromMax - fromMin)*(toMax - toMin) + toMin;
    }



    private void HandleStringPulledBackTolimit(float midPointLocalZAbs, Vector3 midPointLocalSpace)
    {
        //We specify max pulling limit for the string. We don't allow the string to go any farther than "bowStringStretchLimit"
        if (midPointLocalSpace.x < 0 && midPointLocalZAbs >= bowStringStretchLimit)
        {
            audioSource.Pause();

            strength = 1;
            //Vector3 direction = midPointParent.TransformDirection(new Vector3(0, 0, midPointLocalSpace.z));
            midPointVisualObject.localPosition = new Vector3(-bowStringStretchLimit, 0, 0);
        }
    }

    private void HandleStringPushedBackToStart(Vector3 midPointLocalSpace)
    {
        if (midPointLocalSpace.x >= 0)
        {
            audioSource.pitch = 1;
            audioSource.Stop();
            strength = 0;
            midPointVisualObject.localPosition = Vector3.zero;
        }
    }
}
