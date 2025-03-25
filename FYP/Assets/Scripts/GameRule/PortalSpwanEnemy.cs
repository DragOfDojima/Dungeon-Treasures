using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpwanEnemy : MonoBehaviour
{
    public GameObject go = null;
    private void Awake()
    {
        Invoke("Spawn",3f);   
    }
    private void Spawn()
    {
        Instantiate(go, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
