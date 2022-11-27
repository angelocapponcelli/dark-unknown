using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceStatueFireController : MonoBehaviour
{
    [SerializeField] private GameObject _fireball;
    [SerializeField] private float _velocity = 6f;
    [SerializeField] private Transform _spawnFireballPoint;
    [SerializeField] private float _fireballSecondsEnabled = 0.02f;

    void Fire()
    {
        GameObject fireball = Instantiate(_fireball, _spawnFireballPoint.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody2D>().velocity = Vector2.down * _velocity;

        //Enable fireball after _fireballSecondsEnabled sec 
        StartCoroutine(fireball.GetComponent<Fireball>().DeactivateCollider(_fireballSecondsEnabled));
    }
}
