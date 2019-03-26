using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator _animator;
    public AnimatorStateInfo stateInfo;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetStateFullHashPath()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
    }

    public void SetState(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    public void PlayAnim(string animationName)
    {
        _animator.Play(animationName);
    }


}
