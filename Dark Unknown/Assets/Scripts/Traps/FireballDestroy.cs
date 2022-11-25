using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballDestroy : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(DeactivateCollider(0.05f)); //initially collider is deativate beaause of the collider of the column where it start
    }
    void Destroy()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.SetTrigger("destroy");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    IEnumerator DeactivateCollider(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
