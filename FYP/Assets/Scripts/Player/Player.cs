using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform playerHead;
    public CapsuleCollider bodyCollider;
    [SerializeField] GameObject Gameover;
    public Wave wave;

    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2f;

    private float hp = 100;
    private bool dead;

    private AudioSource audioSource;
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

        if (!photonView.IsMine)
        {
            Destroy(GetComponent<AudioSource>());
            Destroy(bodyCollider);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);
    }

    public float getHp()
    {
        return hp;
    }

    public float getMaxHp()
    {
        return 100; // Adjust this if your maximum health changes
    }

    public void IncreaseHp(float p)
    {
        if (!photonView.IsMine) return;

        if (p < 0)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.clip = damaged;
            if (!audioSource.isPlaying)
                audioSource.Play();
            fullScreenEffect.Damage();
        }

        hp = Mathf.Clamp(hp + p, 0, 100);

        if (hp <= 0 && !dead)
        {
            dead = true;
            photonView.RPC("GameOver", RpcTarget.All);
        }
    }

    [PunRPC]
    void GameOver()
    {
        audioSource.Stop();
        audioSource.clip = lose;
        audioSource.pitch = 1;
        audioSource.Play();
        Gameover.SetActive(true);
        StartCoroutine(WaitForRespawn(3));
    }

    private IEnumerator WaitForRespawn(int time)
    {
        SpawnScoreBoard();
        yield return new WaitForSeconds(time);
        hp = 100;
        dead = false;
        Gameover.SetActive(false);
        ResetData();
    }

    public void AddDealDamage(float dd)
    {
        if (!photonView.IsMine) return;
        TotalDamage += dd;
    }

    public void AddEnemySlayed(int es)
    {
        if (!photonView.IsMine) return;
        TotalEnemySlayed += es;
    }

    public void AddScore(int s)
    {
        if (!photonView.IsMine) return;
        Score += s;
    }

    public void AddCorrectAnswer(int ca)
    {
        if (!photonView.IsMine) return;
        TotalAnswerCorrect += ca;
    }

    public void SpawnScoreBoard()
    {
        if (!photonView.IsMine) return; // Only the local player can spawn their own scoreboard

        float timer = wave.getTimer(); // Ensure wave has a method getTimer()

        float minutes = Mathf.FloorToInt(timer / 60);
        if (minutes > 99) { minutes = 99; }
        float seconds = Mathf.FloorToInt(timer % 60);
        string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        GameObject sb = Instantiate(ScoreBoard, transform.position + forward, Quaternion.identity);
        sb.transform.LookAt(Camera.main.transform.position);
        sb.transform.rotation = Quaternion.Euler(sb.transform.rotation.eulerAngles.x, sb.transform.rotation.eulerAngles.y + 180f, sb.transform.rotation.eulerAngles.z);
        sb.GetComponent<ScoreBoard>().setData(Score, TotalDamage, TotalEnemySlayed, TotalAnswerCorrect, timeFormatted);
        wave.resetWaveCount(); // Make sure wave has this method
    }

    private void ResetData()
    {
        if (!photonView.IsMine) return;
        Score = 0;
        TotalDamage = 0;
        TotalEnemySlayed = 0;
        TotalAnswerCorrect = 0;
        Time = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
            stream.SendNext(Score);
            stream.SendNext(TotalDamage);
            stream.SendNext(TotalEnemySlayed);
            stream.SendNext(TotalAnswerCorrect);
            stream.SendNext(Time);
        }
        else
        {
            hp = (float)stream.ReceiveNext();
            Score = (int)stream.ReceiveNext();
            TotalDamage = (float)stream.ReceiveNext();
            TotalEnemySlayed = (int)stream.ReceiveNext();
            TotalAnswerCorrect = (int)stream.ReceiveNext();
            Time = (string)stream.ReceiveNext();
        }
    }
}