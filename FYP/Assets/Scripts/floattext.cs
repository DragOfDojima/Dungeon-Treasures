using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class floattext : MonoBehaviour
{
    // Start is called before the first frame update
    TMP_Text DText;
    void Start()
    {
        DText = GetComponent<TMP_Text>();
        DText.text = i.ToString();
        Destroy(gameObject,3f);
        transform.position = transform.position + -Camera.main.transform.forward *o ;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position-Camera.main.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y+Time.deltaTime/2, transform.position.z);
        
    }
    int i=0;
    public void setText(int i)
    {
        this.i=i;
    }
    float o = 0;
    public void setOffset(float o)
    {
        this.o = o;
    }
}
