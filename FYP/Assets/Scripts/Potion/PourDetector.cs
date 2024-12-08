using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 20;
    public Transform origin;
    public GameObject streamPrefab;
    public Transform rotationDetector;

    private bool isPouring = false;
    private Stream currentStream = null;
    private Potion potion;
    float lastAngle;
    AudioSource audioSource;
    private void Start()
    {
        potion = GetComponent<Potion>();
        audioSource = GetComponent<AudioSource>();
        lastAngle = rotationDetector.up.y * Mathf.Rad2Deg;
    }
    private void Update()
    {

        bool pourCheck = CalculatePourAngle() < pourThreshold;
        Debug.Log("CalculatePourAngle:" + CalculatePourAngle());
        pourThreshold = 20 - 2*((100 - Mathf.FloorToInt(potion.Getcap()))/10);

        if(isPouring != pourCheck&& potion.Getcap() > 0 && potion.GetOpened())
        {
            isPouring = pourCheck;

            if(isPouring )
            {
                
                StartPour();
            }
            else
            {
                EndPour();
            }
            
        }
        if(currentStream != null&& potion.Getcap() <= 0)
        {
            EndPour();
        }
        
    }

    private void StartPour()
    {
        audioSource.Play();
        print("start");
        potion.setIspouring(true);
        currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        audioSource.Stop();
        print("End");
        potion.setIspouring(false);
        currentStream.End();
        currentStream = null;
    }
    private float CalculatePourAngle()
    {
        if ((lastAngle - rotationDetector.up.y * Mathf.Rad2Deg) > 20f)
        {
            lastAngle = rotationDetector.up.y * Mathf.Rad2Deg;
            return -rotationDetector.up.y * Mathf.Rad2Deg;
        }
        else
        {
            lastAngle = rotationDetector.up.y * Mathf.Rad2Deg;
            return rotationDetector.up.y * Mathf.Rad2Deg;
        }
        
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform); ;
        return streamObject.GetComponent<Stream>();
    }
}