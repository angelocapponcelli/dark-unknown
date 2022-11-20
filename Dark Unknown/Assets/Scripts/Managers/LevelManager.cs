using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private List<GameObject> _roomPool;
    private List<GameObject> _nextRooms;
    private GameObject _currentRoom;
    
    void Awake()
    {
        _roomPool = new List<GameObject>();
        _nextRooms = new List<GameObject>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        LoadRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadRooms()
    {
        _roomPool.AddRange(Resources.LoadAll<GameObject>("Prefabs/Rooms")); //load all rooms

        for (int i = 0; i < _nextRooms.Count; i++)
        {
            _nextRooms[i] = _roomPool[Random.Range(0, _roomPool.Count-1)]; //assign random rooms
        }
    }

    public void SetInitialRoom(GameObject room)
    {
        _currentRoom = room;
        Instantiate(_currentRoom, Vector3.zero, Quaternion.identity);
    }
}
