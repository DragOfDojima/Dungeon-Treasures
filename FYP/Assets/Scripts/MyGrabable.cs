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
    bool isIdel = true;
    bool ishover;
    [SerializeField] AudioSource dropaudioSource;
    void Start()
    {
        if(GetComponent<Animator>()!=null)
        animator = GetComponent<Animator>();

    }

    bool isgrabing;
    bool setup;
    bool setup2;
    private void Update()
    {

        if (grabbable.SelectingPoints.Count > 0)
        {
            transform.localScale = Vector3.one;

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
        //animator.SetBool("end", true);
        rb.isKinematic = false;
        rb.useGravity = true;
        for(int i = 0; i < cols.Length; i++)
        {
            cols[i].isTrigger = false;
        }
        transform.localScale = Vector3.one;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player"&& collision.gameObject.tag != "Weapon"&& FindTopParent(collision.gameObject).name!="Player")
        {
            Debug.Log(collision.gameObject.name);
            dropaudioSource.pitch = Random.Range(0.9f, 1.1f);
            dropaudioSource.Play();
        }
    }

    public GameObject FindTopParent(GameObject child)
    {
        Transform currentTransform = child.transform;

        // Traverse up the hierarchy until we reach the root
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        // Return the topmost parent GameObject
        return currentTransform.gameObject;
    }

}
