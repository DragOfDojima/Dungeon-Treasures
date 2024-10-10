using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class floattext : MonoBehaviour
{
    // Start is called before the first frame update
    TMP_Text DText;
    void Start()
    {
        //DText = GetComponent<TextMeshProUGUI>();
        float i = 2.5f;
        Destroy(gameObject,3f);
        //DText.text = Mathf.Ceil(i).ToString("0");
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position-Camera.main.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y+Time.deltaTime/2, transform.position.z);
    }

    public void setText(float i)
    {
        gameObject.GetComponent<TextMeshProUGUI>().text =Mathf.Ceil(i).ToString("0");
    }
}
