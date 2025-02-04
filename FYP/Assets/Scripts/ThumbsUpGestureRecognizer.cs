using UnityEngine;

public class ThumbsUpGestureRecognizer : MonoBehaviour
{
    public OVRHand rightHand;
    public GameObject menu;

    private bool isMenuOpen = false;
    private bool isThumbsUpDetected = false;
    private float gestureHoldTime = 0.5f;
    private float gestureTimer = 0f;

    void Update()
    {
        if (rightHand.IsTracked)
        {
            if (IsThumbsUp(rightHand))
            {
                gestureTimer += Time.deltaTime;
                if (gestureTimer >= gestureHoldTime && !isThumbsUpDetected)
                {
                    ToggleMenu();
                    isThumbsUpDetected = true;
                }
            }
            else
            {
                gestureTimer = 0f;
                isThumbsUpDetected = false;
            }
        }
    }

    bool IsThumbsUp(OVRHand hand)
    {
        bool isThumbUp = hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb);
        bool areOtherFingersDown =
            !hand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            !hand.GetFingerIsPinching(OVRHand.HandFinger.Middle) &&
            !hand.GetFingerIsPinching(OVRHand.HandFinger.Ring) &&
            !hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky);
        Debug.Log("Tumbp up");
        return isThumbUp && areOtherFingersDown;
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menu.SetActive(isMenuOpen);
    }
}

