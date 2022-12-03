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
    [SerializeField] private EnemyController[] possibleCrystalType;
    [SerializeField] private EnemyController _bossEnemyController;
    [SerializeField] private int numOfCrystals;
    private int _numOfEnemy;
    [SerializeField] private float _spawnTime = 1.0f;
    private List<EnemyController> _enemies = new List<EnemyController>();
    private EnemySpawner _enemySpawner;

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
    private bool _isControlEnabled = true;

    private void Start()
    {
        //Player spawn getting a random point from possible player spawn points
        Player.Instance.SetPosition(_spawnPointsPlayer[Random.Range(0, _spawnPointsPlayer.Length)].position);

        //initialize _enemySpawner and call the coroutine which call the enemySpawner method to spawn all enemies 
        _enemySpawner = GetComponent<EnemySpawner>();
    }
 
    // Update is called once per frame
    private void Update()
    {
        if (!_isControlEnabled) return;
        //Check that all enemies are dead
        if (_enemies == null) return;
        var allDead = false;
        foreach (var t in _enemies)
        {
            if (t.IsDead() == false)
                return;
            allDead = true;
        }

        if (!allDead) return;
        //Done at the end of the room when all enemy are dead
        foreach (var d in _doors)
        {
            d.Open();
        }

        var position = _spawnPointReward.position;
        _rewardSpawned = _roomType switch
        {
            Type.HEALTH => Instantiate(_healthReward, position, Quaternion.identity),
            Type.BOW => Instantiate(_bowReward, position, Quaternion.identity),
            Type.STRENGTH => Instantiate(_strengthReward, position, Quaternion.identity),
            Type.SPEED => Instantiate(_speedReward, position, Quaternion.identity),
            Type.SWORD => Instantiate(_swordReward, position, Quaternion.identity),
            _ => _rewardSpawned
        };
        _isControlEnabled = false;
    }

    public void StartRoom(Type roomType)
    {
        //set up the symbols for the next rooms
        if (LevelManager.Instance.GetRoomsTraversed()+1 < LevelManager.Instance.roomsBeforeBoss)
        {
            var toRemove = _possibleSymbols.Find(x => x.type == Type.BOSS);
            _possibleSymbols.Remove(toRemove);
            foreach (var d in _doors)
            {
                var i = Random.Range(0, _possibleSymbols.Count);
                d.setSymbol(_possibleSymbols[i]);
                _possibleSymbols.RemoveAt(i);
            }
        }
        else
        {
            foreach (var d in _doors)
            {
                d.setSymbol(_possibleSymbols.Find((x) => x.type==Type.BOSS));
            }
        }

        _roomType = roomType;
        if (_roomType == Type.RANDOM) _roomType = (Type)Random.Range(2, 6);
        switch (_roomType)
        {
            case Type.INITIAL:
                _numOfEnemy = 1;
                break;
            //Follower types do same thing at first
            case Type.HEALTH:
            case Type.BOW:
            case Type.SPEED:
            case Type.SWORD:
            case Type.STRENGTH:
                _numOfEnemy = Random.Range(20, 25);
                break;
            case Type.BOSS:
                _numOfEnemy = Random.Range(5, 15);
                StartCoroutine(SpawnBoss());
                //StartCoroutine(SpawnCrystals());
                break;
        }
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        if (numOfCrystals != 0)
        {
            for (int i = 0; i < numOfCrystals; i++)
            {
                yield return new WaitForSeconds(_spawnTime);
                _enemies.Add(_enemySpawner.Spawn(possibleCrystalType[Random.Range(0, possibleCrystalType.Length)]));
            }
        }
        
        //while (_availablePlaces.Count!=0) // uncomment to infinitely spawn enemies until no places are left
        for (int i = 0; i < _numOfEnemy; i++) // uncomment to spawn a fixed amount of enemies
        {
            yield return new WaitForSeconds(_spawnTime);
            _enemies.Add(_enemySpawner.Spawn(_possibleEnemyType[Random.Range(0, _possibleEnemyType.Length)]));
        }
    }

    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(_spawnTime);
        _enemies.Add(EnemySpawner.SpawnBoss(_bossEnemyController, _spawnPointReward));
    }
    
    /*private IEnumerator SpawnCrystals()
    {
        for (int i = 0; i < numOfCrystals; i++) // uncomment to spawn a fixed amount of enemies
        {
            yield return new WaitForSeconds(_spawnTime);
            _enemies.Add(_enemySpawner.Spawn(crystalController));
        }
    }*/

    public void DestroyAllEnemies()
    {
        foreach (var t in _enemies)
        {
            Destroy(t.gameObject);
        }
    }

}
