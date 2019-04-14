using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]

public class EnemyController : MonoBehaviour {


    public int Health = 10;
    private NavMeshAgent _navMeshAgent;
    private PlayerController _playerController;
    private Animator mAnimator;
    public GameObject Player;
    public float EnemyDistanceRun = 4.0f;
    private bool mIsDead = false;
    public GameObject[] ItemsDeadState = null;
    public Vector3 _followDistance;
    public string _enemyName;
    public bool _isBeingAttacked = false;
    public string _status = "";
    private float _meleeRange = 3f;


    // Use this for initialization
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerController = Player.GetComponent<PlayerController>();
        mAnimator = GetComponent<Animator>();
    }


    private bool IsNavMeshMoving
    {
        get
        {
            return _navMeshAgent.velocity.magnitude > 0.1f;
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
                    _navMeshAgent.enabled = false;
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

        //Debug.Log("Agent Stopped " + _navMeshAgent.isStopped);



        if (_isBeingAttacked == true)
        {

            if(IsInMeleeDistance())
            {
                _status = "attacking";
                mAnimator.SetTrigger("attack_1");
            }
            else
            {
                LookAtAttackingPlayer();
                Move();
            }
            


            
        }

        
        if(IsNavMeshMoving)
        {
            _status = "Walking";
        }

    }

    private void Move()
    {
        Vector3 newPos = Player.transform.position;//transform.position - dirToPlayer;
        _navMeshAgent.destination = Player.transform.position;
        _navMeshAgent.stoppingDistance = _playerController.radius;
        mAnimator.SetBool("walk", IsNavMeshMoving);
    }

    public void LookAtAttackingPlayer()
    {
        transform.LookAt(_playerController.transform);
    }

    public bool IsInMeleeDistance()
    {

        if (Vector3.Distance(Player.transform.position, transform.position) <= _meleeRange)
        {
            return true;
        } else
            return false;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _meleeRange);

    }
}
