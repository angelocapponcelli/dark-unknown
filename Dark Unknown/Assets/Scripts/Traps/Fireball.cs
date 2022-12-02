using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Destroy()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Reward>() == null && collision.tag != "Trap") //if it doesn't collide with a reward or with a spikeTrap
        {
            _animator.SetTrigger("destroy");
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    /* Initially, the collider is deactivated due to the collider in the column in which it is located, 
     * otherwise the fireball immediately locates the collider and destroys itself at first.
     * After a while with this method, the collider is enabled and the fireball detects the collision.*/
    public IEnumerator DeactivateCollider(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
