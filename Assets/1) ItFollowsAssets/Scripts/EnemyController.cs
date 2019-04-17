using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
//[RequireComponent(typeof(Rigidbody))]

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private GameObject _weildedWeapon;
    private NavMeshAgent _navMeshAgent;
    private PlayerController _playerController;
    private Animator _animator;
    private bool mIsDead = false;
    private float _meleeRange = 3f;
    private float _magicBowRange = 6f;//tie these to weapons
    private float _aggroRange = 20f;
    private Rigidbody _rigidbody;
    private Vector3 _startPosition;

    public int Health = 10;
    public GameObject Player;
    public float EnemyDistanceRun = 4.0f;
    public GameObject[] ItemsDeadState = null;
    public Vector3 _followDistance;
    public string _enemyName;
    public bool _isBeingAttacked = false;
    public string _status = "";




    // Use this for initialization
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerController = Player.GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        Debug.Log(_startPosition);
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
                    _animator.SetTrigger("death");
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
            _navMeshAgent.isStopped = false;
            if (IsInMeleeDistance())
            {
                _status = "attacking";
                _animator.SetTrigger("attack_1");
            }
            else
            {
                if (IsInAggroDistance())
                {
                    LookAtAttackingPlayer();
                    MoveToPlayer();
                }
                else
                {
                    _navMeshAgent.isStopped = true;
                    _animator.SetBool("run", false);
                    _isBeingAttacked = false;

                    ///run back to starting position and stop animating when out of aggro range
                     MoveToCustomPosition(_startPosition);
                    //_animator.SetBool("run", false);
                    //////////////
                }
            }
            


            
        }

        
        if(IsNavMeshMoving)
        {
            _status = "Walking";
        }

    }

    private void MoveToCustomPosition(Vector3 postition)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = postition;
        _animator.SetBool("run", IsNavMeshMoving);
    }


    private void MoveToPlayer()
    {
        _navMeshAgent.isStopped = false;
        Vector3 newPos = Player.transform.position;//transform.position - dirToPlayer;
        _navMeshAgent.destination = Player.transform.position;
        _navMeshAgent.stoppingDistance = _playerController.radius;
        _animator.SetBool("run", IsNavMeshMoving);
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

    public bool IsInAggroDistance()
    {

        if (Vector3.Distance(Player.transform.position, transform.position) <= _aggroRange)
        {
            return true;
        }
        else
            return false;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _meleeRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _magicBowRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _aggroRange);

    }
}
