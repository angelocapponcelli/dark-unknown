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
    private int _roomsTraversed = 0; //counter to distinguish when the next room is the boss room
    [SerializeField] public int roomsBeforeBoss = 5;
    private GameObject _playerSpawnPoint;
    private Player _player;
    [SerializeField] private Animator animator;
    private static readonly int StartTransition = Animator.StringToHash("Starting");
    
    private int _potionCounter;
    [SerializeField] private int roomsBetweenPotions = 2;

    protected override void Awake()
    {
        base.Awake();
        _roomPool = new List<RoomLogic>();
        _nextRooms = new List<RoomLogic>();

        _potionCounter = roomsBetweenPotions;
        
        _roomPool.AddRange(Resources.LoadAll<RoomLogic>("Rooms/RoomsLevel1/"));
        _bossRoom = Resources.Load<RoomLogic>("Rooms/BossRoom1");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //UIController.Instance.SetRoomText("Tutorial room");
    }

    //from GameManager
    public void SetInitialRoom()
    {
        RoomLogic tmp = Resources.Load<RoomLogic>("Rooms/InitialRoom");
        _currentRoom = Instantiate(tmp, Vector3.zero, Quaternion.identity);
        _currentRoom.StartRoom(RoomLogic.Type.INITIAL);
        UIController.Instance.SetRoomText("Tutorial room");
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

        //Destroy reward and potion if player didn't get it
        foreach (var reward in FindObjectsOfType<Reward>())
        {
            Destroy(reward.gameObject);
        }
        
        //instantiate the new room
        _currentRoom = Instantiate(_nextRooms[roomNumber - 1], Vector3.zero, Quaternion.identity);
        _roomPool.Remove(_nextRooms[roomNumber - 1]);
        _currentRoom.StartRoom(roomType);
        if (roomType == RoomLogic.Type.BOSS)
        {
            _bossRoomAlreadyEntered = true;
            UIController.Instance.SetRoomText("Boss Room");
        }
        else
        {
            UIController.Instance.SetRoomText("Rooms before Boss: " + (roomsBeforeBoss - _roomsTraversed));
        }
        //Instantiate potion every tot rooms
        if (_potionCounter == 0 || roomType == RoomLogic.Type.BOSS)
        {
            _currentRoom.InstantiatePotion();
            _potionCounter = roomsBetweenPotions;
        } else if (_potionCounter > 0)
        {
            Debug.Log("Potion countdown: " + _potionCounter);
            _potionCounter--;
        }
        //load next rooms, only if next is not a boss room
        if (roomType == RoomLogic.Type.BOSS) yield break;
        _nextRooms.Clear();
        _roomsTraversed++;
        LoadRooms();
    }

    private void LoadRooms()
    {
        //UIController.Instance.SetRoomText("Rooms before Boss: "+ (roomsBeforeBoss - _roomsTraversed));

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
