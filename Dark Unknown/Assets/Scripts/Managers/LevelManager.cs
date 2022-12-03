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
    private RoomLogic _bossRoom;
    private bool _bossRoomAlreadyEntered = false;
    private int _roomsTraversed = 0; //counter to distinguish when the next is the boss
    [SerializeField] public int roomsBeforeBoss = 5;
    private GameObject _playerSpawnPoint;
    private Player _player;
    [SerializeField] private Animator animator;
    private static readonly int StartTransition = Animator.StringToHash("Starting");

    protected override void Awake()
    {
        base.Awake();
        _roomPool = new List<RoomLogic>();
        _nextRooms = new List<RoomLogic>();
        
        _roomPool.AddRange(Resources.LoadAll<RoomLogic>("Rooms/RoomsLevel1/"));
        _bossRoom = Resources.Load<RoomLogic>("Rooms/BossRoom1");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UIController.Instance.SetRoomText("Room 0");
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
        //destroy current room
        yield return new WaitForSeconds(1);
        _currentRoom.DestroyAllEnemies();
        _currentRoom.DestroyAllFireballs();
        Destroy(_currentRoom.gameObject);

        //Destroy reward if player didn't get it 
        if (FindObjectOfType<Reward>())
            Destroy(FindObjectOfType<Reward>().gameObject);
        
        //instantiate the new room
        _currentRoom = Instantiate(_nextRooms[roomNumber - 1], Vector3.zero, Quaternion.identity);
        _roomPool.Remove(_nextRooms[roomNumber - 1]);
        _currentRoom.StartRoom(roomType);
        if (roomType == RoomLogic.Type.BOSS) _bossRoomAlreadyEntered = true;

        //load next rooms, only if next is not a boss room
        if (roomType != RoomLogic.Type.BOSS)
        {
            _nextRooms.Clear();
            _roomsTraversed++;
            LoadRooms();
        }
    }

    private void LoadRooms()
    {
        UIController.Instance.SetRoomText("Room "+_roomsTraversed);

        if (_roomsTraversed < roomsBeforeBoss)
        {
            for (int i = 0; i < 3; i++) 
            {
                _nextRooms.Add(_roomPool[Random.Range(0, _roomPool.Count)]); //assign random rooms
            }
        }
        else
        {
            for (int i = 0; i < 3; i++) 
            {
                _nextRooms.Add(_bossRoom); //assign boss room to each door
            }
        }
    }

    public RoomLogic GetCurrentRoom()
    {
        return _currentRoom;
    }

    public int GetRoomsTraversed()
    {
        return _roomsTraversed;
    }

    public bool BossRoomAlreadyDone()
    {
        return _bossRoomAlreadyEntered;
    }
}
