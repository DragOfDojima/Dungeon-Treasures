using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.Universal;

public class FullScreenEffect : MonoBehaviour
{
    [Header("Time status")]
    [SerializeField] private float hurtDisplaytime = 0.5f;
    [SerializeField] private float hurtFadetime = 0.5f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature fullScreenDamage;
    [SerializeField] private Material _material;

    private int vignette = Shader.PropertyToID("_VignettePower");

    private const float vignetteStartamount = 2f;
    private const float vignetteMaxamount = 11f;

    private Coroutine hurtCoroutine; // Reference to the current coroutine

    // Start is called before the first frame update
    void Start()
    {
        fullScreenDamage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Damage();
        }
    }

    public void Damage()
    {
        // Stop any currently running Hurt coroutine
        if (hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }

        // Start a new Hurt coroutine
        hurtCoroutine = StartCoroutine(Hurt());
        Debug.Log("damageEffectStart");
    }

    public IEnumerator Hurt()
    {
        fullScreenDamage.SetActive(true);
        _material.SetFloat(vignette, vignetteStartamount);

        // Wait for the display time before starting to increase the vignette effect
        yield return new WaitForSeconds(hurtDisplaytime);

        // Gradually increase the vignette power to the maximum value
        float elapsedTime = 0f;
        while (_material.GetFloat(vignette) < vignetteMaxamount)
        {
            elapsedTime += Time.deltaTime;

            // Lerp the vignette value toward the max amount
            float lerpedVignette = Mathf.Lerp(vignetteStartamount, vignetteMaxamount, elapsedTime / hurtFadetime);

            _material.SetFloat(vignette, lerpedVignette);
            yield return null;
        }

        // Ensure the vignette reaches exactly the maximum value
        _material.SetFloat(vignette, vignetteMaxamount);

        // Disable the effect after it has finished if needed
        // You can uncomment the following line if you want it to turn off
        // fullScreenDamage.SetActive(false);
    }
}