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
    public GameObject Player;
    public GameObject GatheredItem;
   // Gem gem = new Gem();

    private bool mIscutting = false;

    public GameObject[] ItemsDeadState = null;

    // Use this for initialization
    void Start()
    {

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
                if (Player.GetComponent<PlayerController>().IsAttacking)
                {
                    //if(Pcon.)
                    
                    //mAgent.enabled = false;
                    ///Destroy(GetComponent<Rigidbody>());
                   
                    StartCoroutine(CutTree());



                    // Invoke("ShowItemsDeadState", 1.2f);
                }
            }
        }
    }

    IEnumerator CutTree()
    {
        //<color=red>Fatal error:</color>
        mIscutting = true;
        int logsCut = 0;

        int storedTreeLogs = Random.Range(3, 9);
        
        Debug.Log("<color=red>this tree has " + storedTreeLogs + " logs </color>");
        int wait_time = Random.Range(4, 7);

        if (mIscutting == true)
          {
            for (int i = 0; i != storedTreeLogs; i++)
                {

                int logsleft = storedTreeLogs - i;
                            print("iteration = " +i);
                            //Debug.Log("       I is " + i);
                            Debug.Log(transform.name + " is being cut");
                            Debug.Log("<color=red>this tree has " + logsleft + " logs left</color>");
                            Debug.Log("waiting for " + wait_time + " seconds");
                yield return new WaitForSeconds(wait_time);
                            Debug.Log("<color=green>LOG CUT</color>");


            }
           }
        Destroy(GetComponent<Rigidbody>());
        Debug.Log("mcutting is " + mIscutting);
        mIscutting = false;
        Debug.Log("<color=red> END!!!!!!!!!!!!!!!!!!!!</color>");
    }

    // Update is called once per frame
    void Update()
    {
        if (mIscutting)
            return;
    }
    
}


