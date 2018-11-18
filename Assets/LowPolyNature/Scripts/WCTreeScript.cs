using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class WCTreeScript : MonoBehaviour
{

    // private NavMeshAgent mAgent;

    //private Animator mAnimator;
    private PlayerController Pcon;
    public Inventory Inventory;
    public static Inventory ActiveInventory;
    public GameObject Player;
    public static GameObject ActivePlayer;
    public InventoryItemBase GatheredItem;
   // Gem gem = new Gem();
   private bool mIsCutting = false;
    





    public GameObject[] ItemsDeadState = null;

    // Use this for initialization
    void Start()
    {
        //Gets the active player object in scene - Use ActivePlayer instead of Player
        ActivePlayer = Player;
        ActiveInventory = Inventory;
        //mAnimator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        InteractableItemBase item = collision.collider.gameObject.GetComponent<InteractableItemBase>();
        if (item != null)
        {

            // Hit by a weapon
            if (item.ItemType == EItemType.Weapon)
            {
                if (ActivePlayer.GetComponent<PlayerController>().IsAttacking)
                {

                    //if(Pcon.)

                    //mAgent.enabled = false;
                    ///Destroy(GetComponent<Rigidbody>());
                    if (mIsCutting == false)
                    {
                        StartCoroutine(CutTree());
                    }


                    // Invoke("ShowItemsDeadState", 1.2f);
                }
            }
        }
    }

    IEnumerator CutTree()
    {


        //<color=red>Fatal error:</color>
        mIsCutting = true;
        int logsCut = 0;

        int storedTreeLogs = Random.Range(3, 9);
        
        Debug.Log("<color=red>this tree has " + storedTreeLogs + " logs </color>");
        int wait_time = Random.Range(4, 7);

        if (mIsCutting == true)
          {           
            for (int i = 0; i != storedTreeLogs; i++)
            {

                int logsleft = storedTreeLogs - i;
                print("iteration = " + i);
                //Debug.Log("       I is " + i);
                Debug.Log(transform.name + " is being cut");
                Debug.Log("<color=red>this tree has " + logsleft + " logs left</color>");
                Debug.Log("waiting for " + wait_time + " seconds");
                yield return new WaitForSeconds(wait_time);
                if (GatheredItem != null)
                {

                    var TG = Instantiate(GatheredItem, ActivePlayer.transform.position, ActivePlayer.transform.rotation);

                    if (TG != null)
                    {

                        Debug.Log("TG = " + TG);

                        ActiveInventory.AddItem(TG);
                        Object.Destroy(TG.transform.gameObject);
                        Debug.Log("<color=green>LOG CUT</color>");
                        //Debug.Log("TESTGATHER = " + GatheredItem);

                        //Debug.Log("Char Local Pos = " + transform.localPosition);
                    }
                    else
                    {
                        Debug.Log("TG is null: " + TG);
                    }
                }
                else
                {
                    Debug.Log("GatheredItem is null: " + GatheredItem);
                }


                


            }
        }
        
        Debug.Log("mcutting is " + mIsCutting);
        mIsCutting = false;
        Debug.Log("<color=red> END!!!!!!!!!!!!!!!!!!!!</color>");
        Destroy(transform.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (mIsCutting)
            return;
    }
    
}


