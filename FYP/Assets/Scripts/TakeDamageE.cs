using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
    

public class TakeDamageE : MonoBehaviour
{
    public float intensity = 0;

    private Volume _volume;
    private Vignette _vignette;

    void Start()
    {
        // If the Volume is on a different GameObject, find it in the scene
        _volume = FindObjectOfType<Volume>();

        if (_volume == null)
        {
            Debug.LogError("Volume component not found in the scene!");
            return;
        }

        if (!_volume.profile.TryGet(out _vignette))
        {
            Debug.LogError("Vignette effect not found in Volume Profile! Make sure it is added.");
            return;
        }

        _vignette.active = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(TakeDamageEffect());
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        intensity = 0.4f;
        Debug.Log("Damage effect started. Initial intensity: " + intensity);

        // Enable the Vignette effect
        _vignette.active = true;
        _vignette.intensity.value = intensity;

        yield return new WaitForSeconds(0.4f);

        // Gradually decrease the intensity
        while (intensity > 0)
        {
            intensity -= 0.01f;

            if (intensity < 0) intensity = 0;
            _vignette.intensity.value = intensity;

            yield return new WaitForSeconds(0.1f);
        }

        // Disable the Vignette effect
        _vignette.active = false;
    }
}