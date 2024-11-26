using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingAtk : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    [SerializeField] float dealDanage;
    [SerializeField] LayerMask enemy;
    bool igcol=false;
    public void setDamage(float d)
    {
        dealDanage = d;
    }
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.AddForce(transform.forward   * 5);
        Destroy(gameObject, 5f);
        Invoke("spawn",0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "Player")
        {
            other.GetComponent<ToPlayer>().getplayer().setHp(-dealDanage);
            Destroy(gameObject);
        }
        else if(other.gameObject.layer!=enemy&&igcol==true)
            Destroy(gameObject);
    }

    void spawn()
    {
        igcol=true;
    }
}
