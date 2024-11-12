using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class Arrow : XRGrabInteractable
{
    public float speed = 1000f;
    public Transform tip;
    bool inAir = false;
    Vector3 lastPosition = Vector3.zero;
    private Rigidbody rb;
    public Collider sphereCollider;

    [Header("Particles")]
    public ParticleSystem trailParticle;
    public ParticleSystem hitParticle;
    public TrailRenderer trailRenderer;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        if (inAir)
        {
            lastPosition = tip.position;
        }
    }

    private void Stop()
    {
        inAir = false;
        SetPhysics(false);

        ArrowParticles(false);
    }

    public void Release(float value)
    {
        inAir = true;
        SetPhysics(true);
        StartCoroutine(RotateWithVelocity());

        lastPosition = tip.position;

        ArrowParticles(true);
    }

    private void SetPhysics(bool usePhysics)
    {
        rb.useGravity = usePhysics;
        rb.isKinematic = !usePhysics;
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(rb.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }



    void ArrowParticles(bool release)
    {
        if (release)
        {
            trailParticle.Play();
            trailRenderer.emitting = true;
        }
        else
        {
            trailParticle.Stop(); 
            hitParticle.Play();
            trailRenderer.emitting = false;
        }
    }

}
