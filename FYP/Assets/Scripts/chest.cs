using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class chest : MonoBehaviour
{
    MeshRenderer smr;
    public MeshRenderer smt;
    Animator chestLid;
    Animator animator;
    bool isOpen = false;
    public WeightedRandomList<Transform> lootTable;
    public Transform itemHolder;
    Material[] deadmatList;
    [SerializeField] private AnimatorController deadanimation;
    [SerializeField] private Material deadMat;

    // Start is called before the first frame update
    void Start()
    {
        chestLid = GetComponent<Animator>();
        smr = GetComponent<MeshRenderer>();
        deadmatList = smr.materials;
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
                chestLid.Play("TreasureChest_CLOSE", 0, 0.1f);
                HideItem();
                isOpen =false;
                smr.materials = deadmatList;
                smt.materials = deadmatList;
                Debug.Log("Chest Closed");
                }
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
            animator = s.GetComponent<Animator>();
            animator.Play("spawn");

            Debug.Log("item shown");
        }
    }
}

