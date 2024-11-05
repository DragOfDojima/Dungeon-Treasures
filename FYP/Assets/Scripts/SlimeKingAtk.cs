using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingAtk : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.AddForce(transform.forward   * 5);
        Destroy(gameObject, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
