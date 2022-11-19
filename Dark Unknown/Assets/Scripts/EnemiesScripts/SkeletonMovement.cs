using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    public void MoveSkeleton(Vector2 direction)
    {
        transform.Translate((_speed * Time.deltaTime) * direction);
    }

}
