using System;
using System.Collections;
using System.Collections.Generic;
//using Enemies_Scripts;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    private List<RoomLogic> _roomPool;
    private List<RoomLogic> _nextRooms;
    private RoomLogic _currentRoom;
    private int _roomsTraversed = 0; //counter to distinguish when the next is the boss
    [SerializeField] private int roomsBeforeBoss = 5;
    private GameObject _playerSpawnPoint;
    private Player _player;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Animator animator;
    private static readonly int StartTransition = Animator.StringToHash("Starting");
    //private static readonly int EndTransition = Animator.StringToHash("End");

    void Awake()
    {
        _roomPool = new List<RoomLogic>();
        _nextRooms = new List<RoomLogic>();
        
        _roomPool.AddRange(Resources.LoadAll<RoomLogic>("Rooms/"));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UIController.Instance.SetRoomText("Room 0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //from GameManager
    public void SetInitialRoom()
    {
        RoomLogic tmp = Resources.Load<RoomLogic>("Rooms/InitialRoom");
        _currentRoom = Instantiate(tmp, Vector3.zero, Quaternion.identity);
        _currentRoom.StartRoom(RoomLogic.Type.INITIAL);
        _roomPool.Remove(tmp);
        LoadRooms();
    }
    
    public void SetNewRoom(int roomNumber, RoomLogic.Type roomType)
    {
        animator.SetTrigger(StartTransition);
        StartCoroutine(SetRoom(roomNumber, roomType));
    }

    //from other rooms
    private IEnumerator SetRoom(int roomNumber, RoomLogic.Type roomType)
    {
        yield return new WaitForSeconds(1);
        _currentRoom.DestroyAllEnemies();
        Destroy(_currentRoom.gameObject);

        //Destroy reward if player didn't get it 
        if (FindObjectOfType<Reward>())
            Destroy(FindObjectOfType<Reward>().gameObject);
        _currentRoom = Instantiate(_nextRooms[roomNumber - 1], Vector3.zero, Quaternion.identity);
        _currentRoom.StartRoom(roomType);
        _roomPool.Remove(_nextRooms[roomNumber - 1]);
        //Replaced in RoomLogic
        //_player = Player.Instance;
        //_playerSpawnPoint = _currentRoom.transform.Find("PlayerSpawn").gameObject;
        //_player.transform.position = _playerSpawnPoint.transform.position;
        
        //enemySpawner = Instantiate(enemySpawner, Vector3.zero, quaternion.identity);
        
        //load next rooms
        _nextRooms.Clear();
        LoadRooms();
    }

    private void LoadRooms()
    {
        _roomsTraversed++;
        UIController.Instance.SetRoomText("Room "+_roomsTraversed);
        //TODO: load boss room
        //if (_roomsTraversed < roomsBeforeBoss) ...
        for (int i = 0; i < 3; i++) 
        {
            _nextRooms.Add(_roomPool[Random.Range(0, _roomPool.Count)]); //assign random rooms
        }
        //else...
    }

    public RoomLogic GetCurrentRoom()
    {
        return _currentRoom;
    }
}
