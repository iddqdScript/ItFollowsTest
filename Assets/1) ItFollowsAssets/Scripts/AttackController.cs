using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{


    private AnimationController _animController;
    private PlayerController _playerController;
    public bool _shouldInterrupt = false;


    // Start is called before the first frame update
    void Start()
    {
        _animController = GetComponent<AnimationController>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        ShouldInterrupt();
    }



    public IEnumerator AttackEnemyThenWaitForSeconds(int _wait_time_seconds, int amount)
    {
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
