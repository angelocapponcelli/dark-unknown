using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    private int _currentLevel = 0; //is actually incremented when entering the boss room,
                                   //in order to properly update the resources. For the checkpoint, check both this and
                                   //_roomsTraversed, so that if this is n and _roomsTraversed=roomsBeforeBoss+1,
                                   //then you have to restart from the boss room of Level n-1

    private List<RoomLogic> _roomPool;
    private List<RoomLogic> _nextRooms;
    private RoomLogic _currentRoom;
    private RoomLogic _hubRoom;
    private RoomLogic _bossRoom;
    public List<GameObject> killedRewards;

    //private bool _bossRoomAlreadyEntered = false;
    private int _roomsTraversed = -1; //counter to distinguish when the next room is the boss room
                                    //starts at -1 to account for the hub room
    [SerializeField] public int roomsBeforeBoss = 5;
    [SerializeField] private Animator animator;
    private static readonly int StartTransition = Animator.StringToHash("Starting");

    private int _potionCounter;
    [SerializeField] private int roomsBetweenPotions = 2;

    protected override void Awake()
    {
        base.Awake();
        _roomPool = new List<RoomLogic>();
        _nextRooms = new List<RoomLogic>();
        killedRewards = new List<GameObject>();

        _potionCounter = roomsBetweenPotions;

        AddResources();
    }

    private void AddResources()
    {
        _roomPool.Clear();
        _roomPool.AddRange(Resources.LoadAll<RoomLogic>("Rooms/RoomsLevel" + (_currentLevel+1) + "/"));
        _bossRoom = Resources.Load<RoomLogic>("Rooms/BossRoom" + (_currentLevel+1));
        _hubRoom = Resources.Load<RoomLogic>("Rooms/Hub" + (_currentLevel+1));
    }

    //from GameManager
    public void SetInitialRoom()
    {
        RoomLogic tmp = Resources.Load<RoomLogic>("Rooms/InitialRoom");
        _currentRoom = Instantiate(tmp, Vector3.zero, Quaternion.identity);
        _currentRoom.StartRoom(RoomLogic.Type.INITIAL);
        UIController.Instance.SetRoomText("Tutorial room");
        _roomPool.Remove(tmp);
        LoadHubRooms();
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
        CleanUpRoom();
        
        //instantiate the new room
        _currentRoom = Instantiate(_nextRooms[roomNumber - 1], Vector3.zero, Quaternion.identity);
        _roomPool.Remove(_nextRooms[roomNumber - 1]);
        _currentRoom.StartRoom(roomType);
        if (roomType == RoomLogic.Type.BOSS)
        {
            UIController.Instance.SetRoomText("Boss Room");
        }
        else if (roomType == RoomLogic.Type.HUB)
        {
            UIController.Instance.SetRoomText("Hub Room");
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
        }
        else if (_potionCounter > 0 && roomType != RoomLogic.Type.HUB)
        {
            Debug.Log("Potion countdown: " + _potionCounter);
            _potionCounter--;
        }
        else if (_currentLevel == 2)
        {
            _currentRoom.InstantiatePotion();
        }

        //load next rooms
        _roomsTraversed++;
        if (roomType != RoomLogic.Type.BOSS)
        {
            LoadRooms();
        }
        else
        {
            if (_currentLevel < 3)
            {
                AddResources();
                _roomsTraversed = -1;
                GameManager.Instance.SetPlayerRespawned(false);
                LoadHubRooms(); //next room is always hub
            }
            else
            {
                // Implemented scene change when the final boss dies
                //GameManager.Instance.LoadVictoryScene();
                _nextRooms.Add(_hubRoom);
            }
        }
    }

    private void LoadRooms()
    {
        _nextRooms.Clear();

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
    }

    private void LoadHubRooms()
    {
        _nextRooms.Clear();
        
        for (int i = 0; i < 3; i++)
        {
            _nextRooms.Add(_hubRoom); //assign hub room
        }
    }

    public IEnumerator RestartFromHubRoom(float playerSpeed)
    {
        Player player = GameManager.Instance.player;
        //disable colliders
        BoxCollider2D playerCollider = player.GetComponentInChildren<BoxCollider2D>();
        playerCollider.enabled = false;
        CapsuleCollider2D playerFeetCollider = player.GetComponentInChildren<CapsuleCollider2D>();
        playerFeetCollider.enabled = false;
        
        animator.SetTrigger(StartTransition);
        
        yield return new WaitForSeconds(1);
        CleanUpRoom();
        
        //reset previous status
        player.GetPlayerMovement().SetSpeed(playerSpeed);
        player.GetPlayerMovement().enabled = true;
        player.GetPlayerInput().enabled = true;
        playerCollider.enabled = true;
        playerFeetCollider.enabled = true;
        UIController.Instance.SetHealth(player.GetMaxHealth());
        player.ResetCurrentHealth();

        //instantiate the new room
        _currentLevel--;
        AddResources();
        _currentRoom = Instantiate(_hubRoom, Vector3.zero, Quaternion.identity);
        _currentRoom.StartRoom(RoomLogic.Type.HUB);
        UIController.Instance.SetRoomText("Hub Room");
        UIController.Instance.SetInactiveBossHealth();

        _potionCounter = roomsBetweenPotions;
        _roomsTraversed = 0;
        
        //load next rooms
        LoadRooms();
        GameManager.Instance.SetPlayerRespawned(true);
    }

    private void CleanUpRoom()
    {
        _currentRoom.DestroyAllEnemies();
        _currentRoom.DestroyAllFireballs();
        _currentRoom.DestroyAllCrystals();
        DestroyAllKilledRewards();
        Destroy(_currentRoom.gameObject);

        //destroy reward and potion if player didn't get it
        foreach (var reward in FindObjectsOfType<Reward>())
        {
            Destroy(reward.gameObject);
        }
    }

    private void DestroyAllKilledRewards()
    {
        foreach (GameObject go in killedRewards)
        {
            Destroy(go.gameObject);
        }
        killedRewards.Clear();
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

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }
}