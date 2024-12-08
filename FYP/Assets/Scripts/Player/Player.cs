using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerHead;
    public CapsuleCollider bodyCollider;
    [SerializeField] GameObject Gameover;
    public Wave wave;

    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2f;

    private float hp = 100;
    bool dead;

    AudioSource audioSource;
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip damaged;

    private Color originalColor;
    private Camera playerCamera;

    [SerializeField] VisualEffect effect;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCamera = Camera.main;
        originalColor = playerCamera.backgroundColor;
    }
    private void FixedUpdate()
    {
        bodyCollider.height=Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin,bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x,bodyCollider.height/2,playerHead.localPosition.z);
    }

    public void increaseHp(float p)
    {
        if (p < 0)
        {
            effect.Play();
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.clip = damaged;
            if(!audioSource.isPlaying)
            audioSource.Play();
        }
        if(hp + p > 100)
        {
            hp = 100;
        }else
        if (hp + p < 0)
        {
            hp=0;
            
        }
        else
        hp=hp+p;
    }

    public float getHp()
    {
        return hp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            hp=hp-15;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            hp = hp +15;
        }

        if (hp<=0&&!dead) {
            dead = true;
            audioSource.Stop();
            audioSource.clip = lose;
            audioSource.pitch = 1;
            audioSource.Play();
            Gameover.SetActive(true);
            Debug.Log("gameover");
            wave.resetWaveCount();
            StartCoroutine (wait(3));

            //hp = 100;
            //Gameover.SetActive(false);
        }
    }

    IEnumerator wait(int time)
    {
        yield return new WaitForSeconds(time);
        hp = 100;
        dead = false;
        Gameover.SetActive(false);
    }

    
}
