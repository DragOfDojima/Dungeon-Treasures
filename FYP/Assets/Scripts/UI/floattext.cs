using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class floattext : MonoBehaviour
{
    public TMP_Text DText;
    string i;
    float s;
    
    void Start()
    {
        Destroy(gameObject,3f);
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position-Camera.main.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y+Time.deltaTime/2, transform.position.z);
        
    }
    
    public void setText(int i)
    {
        this.i=i.ToString();
        DText.text = this.i;

    }
    float o = 0;
    public void setOffset(float o)
    {
        this.o = o;
        transform.position = transform.position + -Camera.main.transform.forward * o;

    }

    public void setText(string i)
    {
        this.i = i;
    }

    public void setSize(float s)
    {
        this.s = s;
        DText.fontSize = s;

    }
}
