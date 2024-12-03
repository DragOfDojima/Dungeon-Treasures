using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MyGrabable : MonoBehaviour
{
    Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private Collider[] cols;
    [SerializeField] private List<GameObject> matObjects = new List<GameObject>();
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private bool isSpawner;
    bool isIdel = true;
    bool ishover;
    //[SerializeField] private Vector3 weaponscale= new Vector3(1,1,1);
    void Start()
    {
        //if(GetComponent<Animator>()!=null)
        //animator = GetComponent<Animator>();

    }

    bool isgrabing;
    bool setup;
    bool setup2;
    private void Update()
    {
       if (isSpawner&& grabbable.SelectingPoints.Count > 0)
        {
            spawnWeapon();
        }
        if (grabbable.SelectingPoints.Count > 0)
        {
            isIdel = false;
            isgrabing = true;
            if (!setup)
            {
                setup = true;
                Setup();
            }
        }
        else
        {
            if (isgrabing == true)
            {
                Invoke("setIdel", 3f);
            }
            isgrabing = false;
            if (setup && !setup2)
            {
                setup2 = true;
                Setup();
            }
        }
    }

    public bool getIdel()
    {
        return isIdel;
    }
    void setIdel()
    {
        isIdel = true;
    }

    void Setup()
    {
        //animator.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        for(int i = 0; i < cols.Length; i++)
        {
            cols[i].isTrigger = false;
        }
    }

    public bool getIsGrabing()
    {
        return isgrabing;
    }

    public List<GameObject> getMatObjects()
    {
        return matObjects;
    }

    public void removeMatObject(string name)
    {

        StartCoroutine(RemoveMatObject(name));
    }

    IEnumerator RemoveMatObject(string name)
    {
        while (ishover)
        {
            yield return null;
        }
        for (int i = matObjects.Count - 1; i >= 0; i--)
        {
            if (matObjects[i] != null && matObjects[i].name == name)
            {
                matObjects.RemoveAt(i);
            }
        }
    }
    public void isHovering(bool h)
    {
        ishover = h;
    }

    public void spawnWeapon()
    {
        Instantiate(spawnPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(this);
    }
}
