using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerHead;
    public CapsuleCollider bodyCollider;
    
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2f;

    private float hp = 00;
    private void FixedUpdate()
    {
        bodyCollider.height=Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin,bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x,bodyCollider.height/2,playerHead.localPosition.z);
    }

    public void increaseHp(float p)
    {
        if(hp + p > 100)
        {
            hp = 100;
        }else
        if (hp + p < 0)
        {
            hp=0;
        }else
        hp=hp+p;
    }

    public float getHp()
    {
        return hp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            hp=hp-15;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            hp = hp +15;
        }
    }
}
