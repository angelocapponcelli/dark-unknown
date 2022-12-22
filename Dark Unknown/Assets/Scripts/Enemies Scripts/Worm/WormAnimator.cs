using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAnimator : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void AnimateEnemy(bool isMoving, Vector2 direction)
    {
        if (isMoving) flip(direction);
    }

    public void Show()
    {
        _animator.SetTrigger("Exit");
    }

    public void Hide()
    {
        _animator.SetTrigger("Hide");
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }
    
    public void AnimateTakeDamage()
    {
        _animator.SetTrigger("Hurt");
    }
    
    public void AnimateDie()
    {
        _animator.SetFloat("multiplier", 1f);
        _animator.SetTrigger("Dead");
    }
    
    public void AnimateRecover()
    {
        _animator.SetFloat("multiplier", -1f);
        _animator.SetTrigger("Dead");
    }
    
    public void AnimateIdle()
    {
        _animator.SetBool("isMoving", false);
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
    
}
