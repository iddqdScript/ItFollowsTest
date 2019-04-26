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
    private float _meleeRange = 3f;
    private float _magicBowRange = 6f;//tie these to weapons
    public EnemyController _clickedEnemy = null;


    // Start is called before the first frame update
    void Start()
    {
        _animController = GetComponent<AnimationController>();
        _playerController = GetComponent<PlayerController>();
        _navMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(_clickedEnemy != null)
        Debug.Log("Clicked enemy is " + _clickedEnemy.name);
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

    //private bool IsInMeleeDistance()
    //{
    //    if (_clickedEnemy != null)
    //    {
    //        if (Vector3.Distance(_clickedEnemy.transform.position, transform.position) <= _clickedEnemy._meleeRange)
    //        {
    //            return true;
    //        }
    //        else
    //            return false;
    //    }
    //    else return false;
    //}

    //private void MoveToEnemy()
    //{
    //    if (_clickedEnemy != null)
    //    {
    //        _navMesh.isStopped = false;
    //        Vector3 newPos = _clickedEnemy.transform.position;//transform.position - dirToPlayer;
    //        _navMesh.destination = _clickedEnemy.transform.position;
    //        //_navMesh.stoppingDistance = this._magicBowRange;
            
    //        //_animator.SetBool("run", IsNavMeshMoving);
    //    }
    //}

    public void TestingCastingByPressingL()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _animController.PlayAnim("CastSpell");
        }
    }

    private bool ShouldInterrupt()
    {

        //if (_playerController.IsPlayerMoving())
        //{
        //    _shouldInterrupt = true;
        //}
        return true;

    }

}
