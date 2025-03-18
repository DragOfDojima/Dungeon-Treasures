using System.Collections;
using UnityEngine;
using Photon.Pun;

public class SlimeKingAtk : MonoBehaviourPun
{
    private Rigidbody rb;
    private bool trigger;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 5);
        Invoke("StartTrigger", 0.2f);
        Destroy(gameObject, 5f);
    }

    private void StartTrigger()
    {
        trigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger)
        {
            if (other.CompareTag("Player"))
            {
                // Ensure only the master client handles damage
                if (photonView.IsMine)
                {
                    other.GetComponent<ToPlayer>().getplayer().IncreaseHp(-15);
                }
            }

            // Adjust the conditions to destroy the object
            if (other.CompareTag("hitable") && other.name != "impactDamage" && other.name != "Bone")
            {
                Destroy(gameObject);
            }
        }
    }
}