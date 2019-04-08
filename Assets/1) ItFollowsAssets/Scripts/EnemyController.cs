using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {


    public int Health = 10;
    private NavMeshAgent mAgent;
    private PlayerController _playerController;
    private Animator mAnimator;
    public GameObject Player;
    public float EnemyDistanceRun = 4.0f;
    private bool mIsDead = false;
    public GameObject[] ItemsDeadState = null;
    public Vector3 _followDistance;

    // Use this for initialization
    void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();
        _playerController = Player.GetComponent<PlayerController>();
        mAnimator = GetComponent<Animator>();
    }

    private bool IsNavMeshMoving
    {
        get
        {
            return mAgent.velocity.magnitude > 0.1f;
        }
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
                    mIsDead = true;
                    mAgent.enabled = false;
                    mAnimator.SetTrigger("death");
                    Destroy(GetComponent<Rigidbody>());
                    Debug.Log(transform.name + " is ded");

                    Invoke("ShowItemsDeadState", 1.2f);
                }
            }
        }
    }

    void ShowItemsDeadState()
    {
        // Activate the items
        foreach (var item in ItemsDeadState)
        {
            item.SetActive(true);
        }

        Destroy(GetComponent<CapsuleCollider>());

        // Hide the sheep mesh
        transform.Find("sheep_mesh").GetComponent<SkinnedMeshRenderer>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (mIsDead)
            return;
        Vector3 v = new Vector3(-1,0, -1);

        // Only runaway if player is armed
        // bool isPlayerArmed = Player.GetComponent<PlayerController>().IsArmed;

        // Performance optimization: Thx to kyl3r123 :-)
        //float squaredDist = (transform.position - Player.transform.position).sqrMagnitude;
        //float EnemyDistanceRunSqrt = EnemyDistanceRun * EnemyDistanceRun;

        // Run away from player

        // Vector player to me
        //Vector3 dirToPlayer = transform.position + Player.transform.position;

        Vector3 newPos = Player.transform.position;//transform.position - dirToPlayer;

        mAgent.destination = Player.transform.position;
        mAgent.stoppingDistance = _playerController.radius;

        //mAgent.SetDestination(newPos);




        //Debug.Log("newpos is " + newPos);
        //Debug.Log("Sheep position is " + transform.position);
        //Debug.Log("player position is " + Player.transform.position);







        mAnimator.SetBool("walk", IsNavMeshMoving);

    }
}
