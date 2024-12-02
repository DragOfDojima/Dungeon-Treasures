using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MyGrabable : MonoBehaviour
{
    Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private Collider[] cols;
    [SerializeField] private List<GameObject> matObjects;
    bool isIdel = true;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    bool isgrabing;
    bool setup;
    bool setup2;
    private void Update()
    {
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
        animator.enabled = false;
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

        for(int i = 0;i <matObjects.Count;i++)
        {
            if (matObjects[i].name == name)
            {
                matObjects.Remove(matObjects[i]);
            }
        }
    }

}
