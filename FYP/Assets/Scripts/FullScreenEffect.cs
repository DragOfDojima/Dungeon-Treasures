using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.Universal;

public class FullScreenEffect : MonoBehaviour
{
    [Header("Time status")]
    [SerializeField]private float hurtDisplaytime = 0.5f;
    [SerializeField]private float hurtFadetime = 0.5f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature fullScreenDamage;
    [SerializeField] private Material _material;

    private int vignette = Shader.PropertyToID("_VignettePower");

    private const float vignetteStartamount = 2f;
    private const float vignetteEndamount = 11f;

    // Start is called before the first frame update
    void Start()
    {
        fullScreenDamage.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame) { 
           StartCoroutine(Hurt());    
        }
    }

    public void damage() { 
        StartCoroutine(Hurt());    
        Debug.Log("damageEffectStart");
    }
    public IEnumerator Hurt() { 
        fullScreenDamage.SetActive(true);
        _material.SetFloat(vignette,vignetteStartamount);
        yield return new WaitForSeconds(hurtDisplaytime);

        float elapsedTime =0f;
        while(elapsedTime < hurtFadetime) { 
            elapsedTime += Time.deltaTime;
            
            float lerpedVignette = Mathf.Lerp(vignetteEndamount,0f,(elapsedTime / hurtFadetime));

            _material.SetFloat(vignette, lerpedVignette);
            yield return null;
        }
    }
}
