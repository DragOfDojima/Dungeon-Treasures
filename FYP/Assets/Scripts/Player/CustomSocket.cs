using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;

public class CustomSocket : MonoBehaviour
{
    public LayerMask Layer;
    public GameObject Attach;
    public Material HoverMat;
    private Rigidbody rig;
    public bool Freeze = true;
    public bool wasInSoket = false;

    public UnityEvent SelectEnter;
    public UnityEvent SelectExit;

    private GameObject Target;
    private GameObject hoverObject;

    private void OnTriggerStay(Collider other)
    {
        if ((Layer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Target = other.gameObject;
            HoverObject();

            if (Target.GetComponentInParent<MyGrabable>() != null)
            {
                if (Target.GetComponentInParent<MyGrabable>().getIsGrabing() && wasInSoket)
                {
                    Objectgrabed();
                }

                if (!Target.GetComponentInParent<MyGrabable>().getIsGrabing() && !Target.GetComponentInParent<MyGrabable>().getIdel())
                {
                    PlaceAtSoket();
                }
            }
        }
    }

    private void PlaceAtSoket()
    {
        if (!wasInSoket)
        {
            DestroyHoverObject();

            Target.transform.parent = Attach.transform;
            Target.transform.rotation = Attach.transform.rotation;
            Target.transform.position = Attach.transform.position;

            if (Freeze)
            {
                rig = Target.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.constraints = RigidbodyConstraints.FreezeAll;
                }
            }

            SelectEnter.Invoke();
            wasInSoket = true;
        }
    }

    public void Objectgrabed()
    {
        if (wasInSoket)
        {
            SelectExit.Invoke();
            if (rig != null && Freeze)
            {
                rig.constraints = RigidbodyConstraints.None;
            }
            wasInSoket = false;
        }
    }

    private void HoverObject()
    {
        if (hoverObject == null && !wasInSoket && Target.GetComponent<MyGrabable>() != null && !Target.GetComponent<MyGrabable>().getIdel())
        {
            hoverObject = Instantiate(Target.GetComponent<MyGrabable>().getfakeObject(), Attach.transform.position, Attach.transform.rotation);
            hoverObject.transform.parent = Attach.transform;
            hoverObject.layer = 0;

            var grabable = hoverObject.GetComponent<FakeItem>();
            if (grabable != null)
            {
                //grabable.isHovering(true);
                foreach (GameObject go in grabable.getMatObjects())
                {
                    foreach (MeshRenderer rend in go.GetComponents<MeshRenderer>())
                    {
                        var mats = new Material[rend.materials.Length];
                        for (var j = 0; j < rend.materials.Length; j++)
                        {
                            mats[j] = HoverMat;
                        }
                        rend.materials = mats;
                    }
                }
            }

            foreach (Collider c in hoverObject.GetComponents<Collider>())
            {
                c.enabled = false;
            }
            //hoverObject.GetComponent<Rigidbody>().isKinematic = true;
            

        }
    }

    private void DestroyHoverObject()
    {
        if (hoverObject != null)
        {
            if (Target.GetComponent<MyGrabable>() != null)
            {
                Target.GetComponent<MyGrabable>().isHovering(false);
            }
            Destroy(hoverObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((Layer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            DestroyHoverObject();
        }
    }

    public void Update()
    {
        if (Attach.GetComponentInChildren<MyGrabable>() != null && Attach.GetComponentInChildren<MyGrabable>().getIsGrabing())
        {
            Attach.GetComponentInChildren<MyGrabable>().transform.parent = null; // Detach the object
        }
    }
}