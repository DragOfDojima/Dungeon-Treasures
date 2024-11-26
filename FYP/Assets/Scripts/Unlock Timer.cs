using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public float TimeLeft;
    public bool TimerOn=false;
    public GameObject QuestionP;
    public TMP_Text TimerTXT;
    public GameObject WrongP;



    void Start()
    {
    }

    public void Timer_on() {
        TimerOn = true;

    }
    // Update is called once per frame
    void Update()
    {
        if (TimerOn) { 
            if(TimeLeft > 0) { 
                TimeLeft -= Time.deltaTime;   
                
                
            }
            else { 
                Debug.Log("answer unlock");
                TimeLeft = 10f;
                TimerOn = false;
                QuestionP.SetActive(true);
                WrongP.SetActive(false);
            }
            float seconds = Mathf.FloorToInt(TimeLeft%60);
        TimerTXT.text = "Wrong Answer!\r\nChest Locked" + "\n" + "Unlock Time: " + seconds;

        }
    }
  
        
        
    }
