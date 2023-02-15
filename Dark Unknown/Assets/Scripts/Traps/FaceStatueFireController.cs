using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceStatueFireController : MonoBehaviour
{
    [SerializeField] private GameObject _fireball;
    [SerializeField] private float _velocity = 6f;
    [SerializeField] private Transform _spawnFireballPoint;
    [SerializeField] private float _fireballSecondsEnabled = 0.02f;
    [SerializeField] private int direction = 0;

    private Vector2 _vectorDirection;
    
    void Fire()
    {
        _vectorDirection = direction switch
        {
            0 => Vector2.down,
            1 => Vector2.left,
            2 => Vector2.right,
            _ => _vectorDirection
        };
        var fireball = Instantiate(_fireball, _spawnFireballPoint.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody2D>().velocity = _vectorDirection * _velocity;

        //Enable fireball after _fireballSecondsEnabled sec 
        StartCoroutine(fireball.GetComponent<Fireball>().DeactivateCollider(_fireballSecondsEnabled));
    }
}
