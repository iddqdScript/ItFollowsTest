using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackController : MonoBehaviour
{


    private AnimationController _animController;
    private PlayerController _playerController;
    private NavMeshAgent _navMesh;
    private ButtonListController _buttonListController;
    public bool _shouldInterrupt = false;


    // Start is called before the first frame update
    void Start()
    {
        _animController = GetComponent<AnimationController>();
        _playerController = GetComponent<PlayerController>();
        _navMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //ShouldInterrupt(); //this is causing him to stop his attacking cycle when facing the target to attack
        //should I make it interrupt when I click? instead of when the Character is moving by a milliinch?
    }



    public IEnumerator AttackEnemyThenWaitForSeconds(int _wait_time_seconds, int amount, Transform target)
    {
        _navMesh.transform.LookAt(target);
        _shouldInterrupt = false;
        

        for (int i = 0; i < amount; i++)
        {
           
            if (_shouldInterrupt == true)
            {
                yield break;
            }
            else
            {
                _animController.PlayAnim("Attack_1");
                yield return new WaitForSeconds(_wait_time_seconds);
               
            }
        }
    }



    public void TestingCastingByPressingL()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _animController.PlayAnim("CastSpell");
        }
    }

    private void ShouldInterrupt()
    {

        if (_playerController.IsPlayerMoving())
        {
            _shouldInterrupt = true;
        }

    }

}
