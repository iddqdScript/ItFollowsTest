using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NavigationController : MonoBehaviour
{

    public bool _enableNavigation = false;
    public float RotationSpeed = 240.0f;
    public float JumpSpeed = 7.0f;
    public float Speed = 10.0f;

    private PlayerController _playerController;
    private NavMeshAgent _navMeshAgent;
    private GameObject _activePlayer;
    private AnimationController _animationcontroller;
    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;
    private float Gravity = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        _activePlayer = GameObject.Find("Low Poly Warrior");
        _navMeshAgent = _activePlayer.GetComponent<NavMeshAgent>();
        _playerController = _activePlayer.GetComponent<PlayerController>();
        _animationcontroller = _activePlayer.GetComponent<AnimationController>();
        _characterController = _activePlayer.GetComponent<CharacterController>();

        // _navMeshAgent = GameObject.FindObjectOfType<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EnableNavMesh()
    {     
            _navMeshAgent.GetComponent<NavMeshAgent>().enabled = true;
        
    }

    public void DisableNavMesh()
    {
            _navMeshAgent.GetComponent<NavMeshAgent>().enabled = false;
        
    }

    public void ClickToAutoMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                _navMeshAgent.destination = hit.point;
            }
        }

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _animationcontroller.SetState("run", false);
        }
        else
        {
            _animationcontroller.SetState("run", true);
        }
    }

    public void EnableWASDMove()
    {
        // Get Input for axis
        float _horizontalAxis = Input.GetAxis("Horizontal");//forward & backward
        float _verticalaxis = Input.GetAxis("Vertical");//Left & Right
                                                        // Calculate the forward vector
        Vector3 _camerafacingdirection = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;



        Vector3 move = _verticalaxis * _camerafacingdirection + _horizontalAxis * Camera.main.transform.right;

        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // Calculate the rotation for the player
        move = transform.InverseTransformDirection(move);

        // Get Euler angles
        float turnAmount = Mathf.Atan2(move.x, move.z);

        transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);


        //if holding the torch different animation


        if (_characterController.isGrounded)
        {
            _moveDirection = transform.forward * move.magnitude;
            _moveDirection *= Speed;

            if (Input.GetButton("Jump"))
            {
                _animationcontroller.SetState("is_in_air", true);
                //_animator.SetBool("is_in_air", true);
                _moveDirection.y = JumpSpeed;

            }
            else
            {
                _animationcontroller.SetState("is_in_air", false);
                _animationcontroller.SetState("run", move.magnitude > 0);

            }
        }

        _moveDirection.y -= Gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

}
