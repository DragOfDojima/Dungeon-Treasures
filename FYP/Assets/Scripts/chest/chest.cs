using System.Collections;
using UnityEngine;
using Photon.Pun;

public class chest : MonoBehaviourPun
{
    public MeshRenderer mr;
    Animator chestLid;
    Animator animator;
    private bool isOpen = false;
    public WeightedRandomList<Transform> lootTable;
    public Transform itemHolder;
    Material[] deadmatList;
    [SerializeField] private Animator deadanimation;
    [SerializeField] private Material deadMat;
    public GameObject fullChest;
    public GameObject Question;
    public QuestionGame QuestionGame;
    public bool QuestStart = false;
    public bool SG = false;
    GameObject spawnItem;
    AudioSource audioSource;
    [SerializeField] AudioClip opens;
    GameObject theItem;
    chestspawner chestspawner;
    [SerializeField] bool isPotionChest;

    Player whoOpening;

    void Start()
    {
        chestspawner = GameObject.Find("chestSpawner").GetComponent<chestspawner>();
        audioSource = GetComponent<AudioSource>();
        chestLid = GetComponent<Animator>();
        deadmatList = mr.materials;
        for (int i = 0; i < deadmatList.Length; i++)
        {
            deadmatList[i] = deadMat;
        }
    }

    bool closeed = false;
    void Update()
    {
        if (!photonView.IsMine) return; // Only the local player interacts with chests

        if (theItem != null)
        {
            if (theItem.GetComponent<MyGrabable>().getFirstTouch() && !closeed)
            {
                closeed = true;
                photonView.RPC("CloseChestRPC", RpcTarget.All);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; // Only the local player interacts with chests

        if (isOpen == false && other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponentInParent<Player>() != null)
            {
                whoOpening = other.gameObject.GetComponentInParent<Player>();
                if (SG == false)
                {
                    SG = true;
                    StartCoroutine(WaitUntilTrue());
                    Question.SetActive(true);
                }
            }
        }
    }

    [PunRPC]
    public void OpenChestRPC()
    {
        if (isOpen == false)
        {
            audioSource.clip = opens;
            audioSource.Play();
            chestLid.Play("TreasureChest_OPEN", 0, 0.1f);
            ShowItem();
            whoOpening.AddCorrectAnswer(1);
            isOpen = true;
        }
    }

    [PunRPC]
    public void CloseChestRPC()
    {
        StartCoroutine(CloseChestCoroutine());
    }

    IEnumerator CloseChestCoroutine()
    {
        chestLid.Play("TreasureChest_CLOSE", 0, 0.1f);
        HideItem();
        isOpen = false;
        yield return new WaitForSeconds(1);
        fullChest.SetActive(true);
        gameObject.SetActive(false);
        mr.materials = deadmatList;
        deadanimation.enabled = true;
        Destroy(transform.parent.gameObject, 2);
    }

    public void HideItem()
    {
        itemHolder.gameObject.SetActive(false);
        foreach (Transform child in itemHolder)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowItem()
    {
        Transform item = lootTable.GetRandom();
        itemHolder.gameObject.SetActive(true);
        var s = Instantiate(item, new Vector3(itemHolder.position.x, itemHolder.position.y, itemHolder.position.z), Quaternion.identity);
        s.GetComponent<Animator>().enabled = true;
        s.GetComponent<itemSpawnAnimation>().setChest(this);
        spawnItem = s.gameObject;
    }

    public void setTheItem(GameObject i)
    {
        theItem = i;
    }

    public IEnumerator WaitUntilTrue()
    {
        while (!QuestStart)
        {
            if (!SG)
            {
                yield break;
            }
            yield return null;
        }
        ProceedToNextStep();
    }

    public void ProceedToNextStep()
    {
        photonView.RPC("OpenChestRPC", RpcTarget.All);
    }

    public void closeChest()
    {
        if (isPotionChest)
            chestspawner.potionChestClose();
        else
            chestspawner.chestClose();
        photonView.RPC("CloseChestRPC", RpcTarget.All);
    }

    public void answerWrong()
    {
        SG = false;
        whoOpening = null;
    }
}