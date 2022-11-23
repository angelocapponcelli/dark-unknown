using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SkeletonAnimator : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("canMove", true);
    }

    public void AnimateSkeleton(bool isMoving, Vector2 direction)
    {
        _animator.SetBool("isMoving", isMoving);
        if (isMoving) flip(direction);
    }

    public void AnimateAttack(Vector2 direction)
    {
        //flip(direction); move flip of the sprite in the SkeletonController
        _animator.SetBool("canMove", false);
        _animator.SetTrigger("Attack");
    }

    public void AnimateTakeDamage()
    {
        _animator.SetTrigger("Hurt");
    }
    
    public void AnimateIdle()
    {
        _animator.SetBool("isMoving", false);
    }

    public void AnimateDie()
    {
        _animator.SetTrigger("Death");
    }

    public void AnimateRecover()
    {
        _animator.SetTrigger("Recover");
    }

    public void flip(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle) < 90)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x) * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }

    public AnimatorStateInfo GetCurrentState()
    {
        return _animator.GetCurrentAnimatorStateInfo(0);
    }

    public void canMove()
    {
        _animator.SetBool("canMove", true);
    }
}
