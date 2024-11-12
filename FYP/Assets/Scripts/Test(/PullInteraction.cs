using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PullInteraction : XRBaseInteractable
{
    private Bow bow;
    public float PullAmount { get; private set; } = 0.0f;
    public Transform start, end;
    private XRBaseInteractor pullingInteractor = null;
    private float PullAmountTemp;

    [Header("Polish")]
    public LineRenderer stringLine;
    [ColorUsage(true, true)]
    public Color stringNormalCol, stringPulledCol;
    public ParticleSystem lineParticle;
    public bool showHandsOnPull = true;
    public GameObject rightHand, leftHand;

    protected override void Awake()
    {
        base.Awake();
        bow = GetComponentInParent<Bow>();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                Vector3 pullPosition = pullingInteractor.transform.position;
                PullAmount = CalculatePull(pullPosition);

                //haptic
               

                //polish
                stringLine.material.SetColor("_EmissionColor",
                    Color.Lerp(stringNormalCol, stringPulledCol, PullAmount));
            }
        }
    }
    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }



}
