using UnityEditor.Animations;
using System.Collections;
using UnityEngine;

public class chest : MonoBehaviour
{
    public MeshRenderer mr;
    Animator chestLid;
    Animator animator;
    bool isOpen = false;
    public WeightedRandomList<Transform> lootTable;
    public Transform itemHolder;
    Material[] deadmatList;
    [SerializeField] private Animator deadanimation;
    [SerializeField] private Material deadMat;
    public GameObject fullChest;
    public GameObject Question;
    public QuestionGame QuestionGame;
    public bool QuestStart = false;
    public bool SG=false;
    GameObject spawnItem;
    AudioSource audioSource;
    [SerializeField] AudioClip opens;

    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        chestLid = GetComponent<Animator>();
        deadmatList = mr.materials;
        for (int i = 0; i < deadmatList.Length; i++)
        {
            deadmatList[i] = deadMat;
        }
        StartCoroutine(WaitUntilTrue());
    }

    // Update is called once per frame
    bool closeed=false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowItem();
        }
        
        if (spawnItem != null) {
            if (spawnItem.GetComponent<MyGrabable>().getIsGrabing()&&!closeed) 
            {
                closeed = true;
                StartCoroutine(close());
            }
        }
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isOpen == false)
        {
            if (other.gameObject.tag == "Player")
            {
                if (SG == false) {
                    SG = true;
                    Question.SetActive(true);
                }
            }
        }
    }

    public void open() {
        if (isOpen == false)
        {
            audioSource.clip = opens;
            audioSource.Play();
            chestLid.Play("TreasureChest_OPEN", 0, 0.1f);
            ShowItem();
            isOpen = true;
        }
        else
        {
            
        }

    }

    public IEnumerator close()
    {

        chestLid.Play("TreasureChest_CLOSE", 0, 0.1f);
        HideItem();
        isOpen = false;
        yield return new WaitForSeconds(1);
        fullChest.SetActive(true);
        gameObject.SetActive(false);
        mr.materials = deadmatList;
        deadanimation.enabled = true;
        Destroy(transform.parent.gameObject,2);
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
        spawnItem = s.gameObject;
    }

    public IEnumerator WaitUntilTrue()
    {
        while (!QuestStart)
        {
            yield return null;
        }
        ProceedToNextStep();
    }

    public void ProceedToNextStep()
    {
        open();
    }

    public void closeChest()
    {
        StartCoroutine(close());
    }
}
