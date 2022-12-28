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
    private int _currentLevel = 1; //is actually incremented when entering the boss room,
                                   //in order to properly update the resources. For the checkpoint, check both this and
                                   //_roomsTraversed, so that if this is n and _roomsTraversed=roomsBeforeBoss+1,
                                   //then you have to restart from the boss room of Level n-1

    private List<RoomLogic> _roomPool;
    private List<RoomLogic> _nextRooms;
    private RoomLogic _currentRoom;

    private RoomLogic _bossRoom;

    //private bool _bossRoomAlreadyEntered = false;
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

        AddResources();
    }

    // Start is called before the first frame update
    void Start()
    {
        //UIController.Instance.SetRoomText("Tutorial room");
    }

    public void AddResources()
    {
        print("currentLevel: " + _currentLevel);
        _roomPool.Clear();
        _roomPool.AddRange(Resources.LoadAll<RoomLogic>("Rooms/RoomsLevel" + _currentLevel + "/"));
        _bossRoom = Resources.Load<RoomLogic>("Rooms/BossRoom" + _currentLevel);
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
        _currentRoom.DestroyAllCrystals();
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
            //_bossRoomAlreadyEntered = true;
            UIController.Instance.SetRoomText("Boss Room");
        }
        else
        {
            UIController.Instance.SetRoomText("Rooms before Boss: " + (roomsBeforeBoss - _roomsTraversed));
        }

        //Instantiate potion every tot rooms
        if (_potionCounter == 0 || roomType == RoomLogic.Type.BOSS)
        {
            //TODO
            //_currentRoom.InstantiatePotion();
            _potionCounter = roomsBetweenPotions;
        }
        else if (_potionCounter > 0)
        {
            Debug.Log("Potion countdown: " + _potionCounter);
            _potionCounter--;
        }

        //load next rooms
        _roomsTraversed++;
        print("Rooms traversed: " + _roomsTraversed);
        LoadRooms();
    }

    private void LoadRooms()
    {
        _nextRooms.Clear();
        //UIController.Instance.SetRoomText("Rooms before Boss: "+ (roomsBeforeBoss - _roomsTraversed));

        if (_roomsTraversed < roomsBeforeBoss)
        {
            for (int i = 0; i < 3; i++)
            {
                _nextRooms.Add(_roomPool[Random.Range(0, _roomPool.Count)]); //assign random rooms
            }
        }
        else if (_roomsTraversed == roomsBeforeBoss)
        {
            for (int i = 0; i < 3; i++)
            {
                _nextRooms.Add(_bossRoom); //assign boss room to each door
            }
        }
        else
        {
            if (_currentLevel < 3)
            {
                AddResources();
                _roomsTraversed = 0;
                for (int i = 0; i < 3; i++)
                {
                    _nextRooms.Add(_roomPool[Random.Range(0, _roomPool.Count)]); //assign random rooms
                }
            }
            else
            {
                GameManager.Instance.LoadVictoryScene();
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

    public void IncrementCurrentLevel()
    {
        _currentLevel++;
    }
}