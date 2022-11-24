using System;
using System.Collections;
using System.Collections.Generic;
using Enemies_Scripts;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private List<GameObject> _roomPool;
    private List<GameObject> _nextRooms;
    private GameObject _currentRoom;
    private int _roomsTraversed = 0; //counter to distinguish when the next is the boss
    [SerializeField] private int roomsBeforeBoss = 5;
    private GameObject _playerSpawnPoint;
    private Player _player;
    [SerializeField] private EnemySpawner enemySpawner;
    
    void Awake()
    {
        _roomPool = new List<GameObject>();
        _nextRooms = new List<GameObject>();
        
        _roomPool.AddRange(Resources.LoadAll<GameObject>("Rooms/"));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //from GameManager
    public void SetInitialRoom()
    {
        GameObject tmp = _roomPool[Random.Range(0, _roomPool.Count)];
        _currentRoom = Instantiate(tmp, Vector3.zero, Quaternion.identity);
        _roomPool.Remove(tmp);
        LoadRooms();
    }

    //from other rooms
    public void SetRoom(int roomNumber)
    {
        Destroy(_currentRoom);
        //Destroy(enemySpawner);
        _currentRoom = Instantiate(_nextRooms[roomNumber - 1], Vector3.zero, Quaternion.identity);
        _roomPool.Remove(_nextRooms[roomNumber - 1]);
        _player = FindObjectOfType<Player>();
        _playerSpawnPoint = _currentRoom.transform.Find("PlayerSpawn").gameObject;
        _player.transform.position = _playerSpawnPoint.transform.position;
        //enemySpawner = Instantiate(enemySpawner, Vector3.zero, quaternion.identity);
        
        //load next rooms
        _nextRooms.Clear();
        LoadRooms();
    }
    
    private void LoadRooms()
    {
        _roomsTraversed++;

        //TODO: load boss room
        //if (_roomsTraversed < roomsBeforeBoss) ...
        for (int i = 0; i < 3; i++) 
        {
            _nextRooms.Add(_roomPool[Random.Range(0, _roomPool.Count)]); //assign random rooms
        }
        //else...
        
        
    }

    public GameObject GetCurrentRoom()
    {
        return _currentRoom;
    }
}
