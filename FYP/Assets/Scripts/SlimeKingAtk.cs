using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingAtk : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    bool trigger;
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.AddForce(transform.forward   * 5);
        Invoke("startTrigger", 0.2f);
        Destroy(gameObject, 5f);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void startTrigger()
    {
        trigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(trigger)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<ToPlayer>().getplayer().increaseHp(-15);
            }
            if (other.tag != "hitable" || other.name != "impactDamage" || other.name != "Bone")
                Destroy(gameObject);
        }
        
    }
}
