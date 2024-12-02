using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private ParticleSystem splashParticle;

    private Coroutine pourRoutine;
    private Vector3 targetPosition = Vector3.zero;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    private IEnumerator BeginPour()
    {
        while (gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }
    }

    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    private IEnumerator EndPour()
    {
        while (!HasReachedPosition(0, targetPosition))
        {
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }

        Destroy(gameObject);
    }
    bool heal;
    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit,1.0f);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Player" && heal == false)
            {
                heal = true;
                if (hit.collider.gameObject.GetComponent<ToPlayer>() != null)
                {
                    hit.collider.gameObject.GetComponent<ToPlayer>().getplayer().increaseHp(2f);
                }
                else
                {
                    hit.collider.gameObject.GetComponentInParent<ToPlayer>().getplayer().increaseHp(2f);
                }
                
                StartCoroutine(healcolddown());
            }
        }
        return endPoint;
    }

    private IEnumerator healcolddown()
    {

        yield return new WaitForSeconds(0.048f);
        heal = false;
    }
    private void MoveToPosition(int index, Vector3 targetPos)
    {
        lineRenderer.SetPosition(index, targetPos);
    }

    private void AnimateToPosition(int index, Vector3 targetPos)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPos, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }

    private bool HasReachedPosition(int index, Vector3 tartePos)
    {
        Vector3 currentPostion = lineRenderer.GetPosition(index);
        return currentPostion == targetPosition;
    }

    private IEnumerator UpdateParticle()
    {
        while(gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetPosition;

            bool isHitting = HasReachedPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);

            yield return null;
        }
    }
}
