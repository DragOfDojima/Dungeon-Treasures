using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChest : MonoBehaviour
{
    bool haveChest=false;
    bool hc = false;
    bool waitSpawn=false;
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
            waitSpawn=false;
        }
        else
        {
            if (!waitSpawn)
            {
                waitSpawn=true;
                StartCoroutine(chestRespawnTime());
            }
            
        }
    }

    IEnumerator chestRespawnTime()
    {
        yield return new WaitForSeconds(WaitTime);
        haveChest=false;
        waitSpawn=false;
    }

    public bool getHaveChest()
    {
        return haveChest;
    }

    public void setHaveChest(bool value)
    {
        haveChest=value;
    }

    public void setWaitTime(float t)
    {
        WaitTime = t;
    }
}
