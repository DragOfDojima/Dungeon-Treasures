using System.Collections;
using UnityEngine;

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
    [SerializeField] FullScreenEffect fullScreenEffect;

    public GameObject ScoreBoard;

    private int Score;
    private float TotalDamage;
    private int TotalEnemySlayed;
    private int TotalAnswerCorrect;
    private string Time;
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
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.clip = damaged;
            if(!audioSource.isPlaying)
            audioSource.Play();
            fullScreenEffect.Damage();

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
            increaseHp(-15);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            hp = hp +15;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            spawnScoreBoard();
        }

        if (hp<=0&&!dead) {
            dead = true;
            audioSource.Stop();
            audioSource.clip = lose;
            audioSource.pitch = 1;
            audioSource.Play();
            Gameover.SetActive(true);
            StartCoroutine(wait(3));

            //hp = 100;
            //Gameover.SetActive(false);
        }
    }


    IEnumerator wait(int time)
    {
        spawnScoreBoard();
        yield return new WaitForSeconds(time);
        hp = 100;
        dead = false;
        Gameover.SetActive(false);
        resetData();
    }

    public void addDealDamage(float dd)
    {
        TotalDamage += dd;
    }

    public void addEnermySlayed(int es)
    {
        TotalEnemySlayed += es;
    }

    public void addScore(int s)
    {
        Score += s;
    }

    public void addCorrectAnswer(int ca)
    {
        TotalAnswerCorrect += ca;
    }

    public void spawnScoreBoard()
    {
        float timer = wave.getTimer();

        float minutes = Mathf.FloorToInt(timer / 60);
        if (minutes > 99) { minutes = 99; }
        float seconds = Mathf.FloorToInt(timer % 60);
        Time = string.Format("{0:00}:{1:00}", minutes, seconds);
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        Vector3 position = GameObject.Find("GameM").GetComponent<StartMenuToCenter>().getCenterOfRoom();
        GameObject sb = Instantiate(ScoreBoard, position, Quaternion.identity);
        sb.transform.LookAt(Camera.main.transform.position);
        sb.transform.rotation = Quaternion.Euler(sb.transform.rotation.eulerAngles.x, sb.transform.rotation.eulerAngles.y+180f, sb.transform.rotation.eulerAngles.z);
        sb.GetComponent<ScoreBoard>().setData(Score, TotalDamage, TotalEnemySlayed, TotalAnswerCorrect, Time);
        wave.resetWaveCount();
    }

    void resetData()
    {
        Score = 0;
        TotalDamage = 0;
        TotalEnemySlayed = 0;
        TotalAnswerCorrect = 0;
        Time = null;
    }
}
