using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private LevelManager _levelManager;
    [SerializeField] private Player player;
    private CinemachineVirtualCamera _cineMachine;
    private GameObject _currentRoom;
    private GameObject _playerSpawnPoint;
    
    protected override void Awake()
    {
        base.Awake();
        _levelManager = FindObjectOfType<LevelManager>();
        _cineMachine = FindObjectOfType<CinemachineVirtualCamera>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _levelManager.SetInitialRoom();
        _currentRoom = _levelManager.GetCurrentRoom();
        _playerSpawnPoint = _currentRoom.transform.Find("PlayerSpawn").gameObject;
        player = Instantiate(player,_playerSpawnPoint.transform.position,Quaternion.identity);
        _cineMachine.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 0, 0);
    }
}
