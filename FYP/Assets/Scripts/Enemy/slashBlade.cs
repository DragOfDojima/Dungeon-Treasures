using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class slashBlade : MonoBehaviour
{
    public int Damage = 20;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 3.8f * Time.deltaTime);
        if(transform.localPosition.z>= 4.37f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided" + other.gameObject.name);
        if (other.tag == "Shield")
        {
            if (other.tag == "Shield")
            {
                other.GetComponent<weapon>().shield();
            }
            Destroy(gameObject);
        }
        if (other.tag == "Player" )
        {
            if (other.name == "HandOverAll") return;

            other.GetComponent<ToPlayer>().getplayer().increaseHp(-Damage);

        }

    }
}

