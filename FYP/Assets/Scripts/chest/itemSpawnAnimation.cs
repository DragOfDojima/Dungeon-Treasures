
using UnityEngine;


public class itemSpawnAnimation : MonoBehaviour
{
    //give this script to item
    Animator animator;
    [SerializeField] GameObject toSpawn;
    GameObject spawned;
    chest chest;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("New State"))
        {
            spawned = Instantiate(toSpawn, new Vector3(transform.position.x, transform.position.y,transform.position.z), Quaternion.identity);
            chest.setTheItem(spawned);
            Destroy(gameObject);
        }

    }

    public GameObject getSpawnedItem()
    {
        return spawned;
    }

    public void setChest(chest c)
    {
        chest = c;
    }
}
