using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject[] waypoints;
    private int _currentWaypointIndex = 0;

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(waypoints[_currentWaypointIndex].transform.position, 
                transform.position) < .1f)
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[_currentWaypointIndex].transform.position,
            Time.deltaTime * speed);
    }
}
