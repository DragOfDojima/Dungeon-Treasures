using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChest : MonoBehaviour
{
    bool haveChest=false;
    bool hc = false;

    public LayerMask whatIsChest;
    float WaitTime=10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hc=Physics.CheckSphere(transform.position, 1, whatIsChest);
        if (hc)
        {
            haveChest=true;
        }
        else
        {
            StartCoroutine(chestRespawnTime());
        }
    }

    IEnumerator chestRespawnTime()
    {
        yield return new WaitForSeconds(WaitTime);
        haveChest=false;
    }

    public bool getHaveChest()
    {
        return haveChest;
    }

    public void setWaitTime(float t)
    {
        WaitTime = t;
    }
}
