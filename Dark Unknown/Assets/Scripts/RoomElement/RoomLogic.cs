using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomLogic : MonoBehaviour
{
    [Header ("Player spawner")]
    [SerializeField] private Transform[] _spawnPointsPlayer;

    [Header("Enemy spawner")]
    [SerializeField] private EnemyController[] _possibleEnemyType;
    [SerializeField] private EnemyController _bossEnemyController;
    private int _numOfEnememy;
    [SerializeField] private float _spawnTime = 1.0f;
    private List<EnemyController> _enemies = new List<EnemyController>();
    private EnemySpawner _enemySpawner;
    //private BossSpawner _bossSpawner;

    [Header("Doors")]
    [SerializeField] private Door[] _doors;
    [SerializeField] private List<SymbolType> _possibleSymbols = new List<SymbolType>();

    [Header("Rewards")]
    [SerializeField] private HealthReward _healthReward;
    [SerializeField] private SpeedReward _speedReward;
    [SerializeField] private BowReward _bowReward;
    [SerializeField] private StrengthReward _strengthReward;
    [SerializeField] private SwordReward _swordReward;

    [SerializeField] private Transform _spawnPointReward;
    private Reward _rewardSpawned;
    
    public enum Type {INITIAL, RANDOM, HEALTH, BOW, SPEED, STRENGTH, SWORD, BOSS};
    private Type _roomType;
    private bool _isControllEnabled = true;

    private void Start()
    {
        //Player spawn getting a random point from possible player spawn points
        Player.Instance.SetPosition(_spawnPointsPlayer[Random.Range(0, _spawnPointsPlayer.Length)].position);

        //initialize _enemySpawner and call the coroutine which call the enemySpawner method to spawn all enemies 
        _enemySpawner = GetComponent<EnemySpawner>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if (_isControllEnabled)
        {
            //Check that all enemies are dead
            if (_enemies != null)
            {
                bool allDead = false;
                for (int i = 0; i < _enemies.Count; i++)
                {
                    if (_enemies[i].IsDead() == false)
                        return;
                    allDead = true;
                }
                if (allDead)
                {
                    //Done at the end of the room when all enemy are dead
                    foreach (Door d in _doors)
                    {
                        d.Open();
                    }                    
                    switch (_roomType)
                    {
                        case Type.HEALTH:
                            _rewardSpawned = Instantiate(_healthReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                        case Type.BOW:
                            _rewardSpawned = Instantiate(_bowReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                        case Type.STRENGTH:
                            _rewardSpawned = Instantiate(_strengthReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                        case Type.SPEED:
                            _rewardSpawned = Instantiate(_speedReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                        case Type.SWORD:
                            _rewardSpawned = Instantiate(_swordReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                    }
                    _isControllEnabled = false;
                }
            }
        }     
    }

    public void StartRoom(Type roomType)
    {
        //set up the symbols for the next rooms
        if (LevelManager.Instance.GetRoomsTraversed()+1 < LevelManager.Instance.roomsBeforeBoss)
        {
            SymbolType toRemove = _possibleSymbols.Find(x => x.type == Type.BOSS);
            _possibleSymbols.Remove(toRemove);
            foreach (Door d in _doors)
            {
                int i = Random.Range(0, _possibleSymbols.Count);
                d.setSymbol(_possibleSymbols[i]);
                _possibleSymbols.RemoveAt(i);
            }
        }
        else
        {
            foreach (Door d in _doors)
            {
                d.setSymbol(_possibleSymbols.Find((x) => x.type==Type.BOSS));
            }
        }

        _roomType = roomType;
        if (_roomType == Type.RANDOM) _roomType = (Type)Random.Range(2, 6);
        switch (_roomType)
        {
            case Type.INITIAL:
                _numOfEnememy = 1;
                break;
            //Follower types do same thing at first
            case Type.HEALTH:
            case Type.BOW:
            case Type.SPEED:
            case Type.SWORD:
            case Type.STRENGTH:
                _numOfEnememy = Random.Range(20, 25);
                break;
            case Type.BOSS:
                _numOfEnememy = Random.Range(5, 15);
                StartCoroutine(spawnBoss());
                break;
        }
        StartCoroutine(spawnEnemies());
    }

    private IEnumerator spawnEnemies()
    {
        //while (_availablePlaces.Count!=0) // uncomment to infinitely spawn enemies until no places are left
        for (int i = 0; i < _numOfEnememy; i++) // uncomment to spawn a fixed amount of enemies
        {
            yield return new WaitForSeconds(_spawnTime);
            _enemies.Add(_enemySpawner.Spawn(_possibleEnemyType[Random.Range(0, _possibleEnemyType.Length)]));
        }
    }

    private IEnumerator spawnBoss()
    {
        yield return new WaitForSeconds(_spawnTime);
        _enemies.Add(_enemySpawner.SpawnBoss(_bossEnemyController, _spawnPointReward));
    }

    public void DestroyAllEnemies()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            Destroy(_enemies[i].gameObject);
        }
    }

}
