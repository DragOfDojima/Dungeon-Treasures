using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bow : XRGrabInteractable
{
    public Transform notch;
    private PullInteraction pullInteraction;
    private LineRenderer lineRenderer;

    [Header("Quiver")]
    public Transform quiver;
    public Vector2 quiverOffset;

    protected override void Awake()
    {
        base.Awake();
        pullInteraction = GetComponentInChildren<PullInteraction>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            if (isSelected)
                UpdateBow(pullInteraction.PullAmount);
    }
    private void UpdateBow(float value)
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(-0.25f, -0.5f, value);
        notch.localPosition = linePosition;
        lineRenderer.SetPosition(1, linePosition);
    }



    //public void BowHaptic(XRBaseInteractor interactor)
    //{
    //    if (interactor.TryGetComponent(out XRController controller))
    //        HapticManager.Impulse(1, .05f, controller.inputDevice);
    //}

}