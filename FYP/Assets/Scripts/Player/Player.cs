using UnityEngine;
using Photon.Pun;
using TMPro; // For TextMeshPro support

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform playerHead;
    public CapsuleCollider bodyCollider;
    public TextMeshProUGUI nameTag; // Reference for the name tag

    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2f;

    private float hp = 100; // Maximum health
    private float currentHP; // Current health
    private bool dead;

    private int score; // Player's score
    private int totalDamage; // Total damage dealt
    private int totalEnemiesSlayed; // Total enemies slayed
    private int correctAnswers; // Count of correct answers

    private void Start()
    {
        currentHP = hp; // Initialize current HP
        if (nameTag != null)
        {
            nameTag.text = PhotonNetwork.NickName; // Set name tag to player's nickname
        }

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

        // Update name tag position
        if (nameTag != null)
        {
            nameTag.transform.position = playerHead.position + new Vector3(0, 1.5f, 0); // Adjust height as needed
            nameTag.transform.LookAt(Camera.main.transform); // Face the camera
        }
    }

    public void IncreaseHp(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, hp); // Ensure health does not exceed max
    }

    public float getHp() => currentHP; // Return current health
    public float getMaxHp() => hp; // Return maximum health

    public void Damage(float damage)
    {
        if (dead) return;

        currentHP -= damage;
        if (currentHP <= 0)
        {
            dead = true;
            // Handle player death (respawn, game over, etc.)
            PhotonNetwork.Destroy(gameObject); // Example of destroying the player object
        }
    }

    public void AddDealDamage(int damage)
    {
        totalDamage += damage;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void AddEnemySlayed(int enemiesSlayed)
    {
        totalEnemiesSlayed += enemiesSlayed;
    }

    public void AddCorrectAnswer(int count)
    {
        correctAnswers += count; // Increment correct answers
    }

    // New method to spawn the scoreboard
    public void SpawnScoreBoard()
    {
        // Implement logic to spawn the scoreboard UI or GameObject
        // For example:
        Debug.Log("Scoreboard spawned for player: " + PhotonNetwork.NickName);
        // You might instantiate a UI prefab or update a score display here
    }

    public bool IsDead() => dead;

    public int GetScore() => score;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHP);
            stream.SendNext(dead);
            stream.SendNext(score);
            stream.SendNext(totalDamage);
            stream.SendNext(totalEnemiesSlayed);
            stream.SendNext(correctAnswers);
        }
        else
        {
            currentHP = (float)stream.ReceiveNext();
            dead = (bool)stream.ReceiveNext();
            score = (int)stream.ReceiveNext();
            totalDamage = (int)stream.ReceiveNext();
            totalEnemiesSlayed = (int)stream.ReceiveNext();
            correctAnswers = (int)stream.ReceiveNext();
        }
    }
}