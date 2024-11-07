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

    // Start is called before the first frame update
    void Start()
    {
        chestLid = GetComponent<Animator>();
        deadmatList = mr.materials;
        for (int i = 0; i < deadmatList.Length; i++)
        {
            deadmatList[i] = deadMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            chestLid.Play("TreasureChest_OPEN",0,0.1f);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            chestLid.Play("TreasureChest_CLOSE", 0, 0.1f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.tag == "Player") { 
            Debug.Log("Open Chest");
            if (isOpen == false) { 
            chestLid.Play("TreasureChest_OPEN", 0, 0.1f);
             ShowItem();
            isOpen=true; 
            }
            
            else {
                StartCoroutine(close());
                }
       }

    IEnumerator close()
        {
            chestLid.Play("TreasureChest_CLOSE", 0, 0.1f);
            HideItem();
            isOpen = false;
            yield return new WaitForSeconds(1);
            fullChest.SetActive(true);
            gameObject.SetActive(false);
            mr.materials = deadmatList;
            deadanimation.enabled = true;
            Debug.Log("Chest Closed");
        }

    void HideItem()
        {
            itemHolder.gameObject.SetActive(false);

            foreach (Transform child in itemHolder)
            {
                Destroy(child.gameObject);
            }
            
        }

        void ShowItem(){
            Transform item = lootTable.GetRandom();
            itemHolder.gameObject.SetActive(true);
            var s = Instantiate(item, itemHolder);
            s.GetComponent<Animator>().enabled=true;
            s.GetComponent<Animator>().Play("spawn");
            Debug.Log("item shown");
        }
    }
}

