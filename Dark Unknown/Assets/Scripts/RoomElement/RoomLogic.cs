using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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
    [SerializeField] private float _spawnTime = 1.0f;
    private List<EnemyController> _enemies = new List<EnemyController>();
    private GameObject[] _projectiles;
    private EnemySpawner _enemySpawner;
    [SerializeField] private float spiderPercentage;

    [Header("Doors")]
    [SerializeField] private Door[] _doors;
    [SerializeField] private List<SymbolType> _possibleSymbols = new List<SymbolType>();

    [Header("Rewards")]
    [SerializeField] private HealthReward _healthReward;
    [SerializeField] private SpeedReward _speedReward;
    [SerializeField] private WeaponReward _bowReward;
    [SerializeField] private StrengthReward _strengthReward;
    [SerializeField] private WeaponReward _swordReward;
    [SerializeField] private WeaponReward _axeReward;
    [SerializeField] private AbilityReward _circleAbilityReward;
    [SerializeField] private AbilityReward _iceProjectileAbility;
    [SerializeField] private AbilityReward _airAttackAbility;
    [SerializeField] private AbilityReward _shieldAbility;

    [SerializeField] private Transform _spawnPointReward;
    [SerializeField] private Transform spawnPointPotion;
    private Reward _rewardSpawned;

    private int _numOfEnemies;
    [HideInInspector] public List<EnemyController> crystals = new List<EnemyController>();

    public enum Type {INITIAL, RANDOM, SPEED, STRENGTH, BOW, SWORD, AXE, BOSS, HUB, 
        CIRCLE_ABILITY, AIRATTACK_ABILITY, ICEPROJECTILE_ABILITY, SHIELD_ABILITY};
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
        bool allDead = false;
        foreach (var t in _enemies)
        {
            if (t.IsDead() == false)
                return;
            allDead = true;
        }

        if (_roomType == Type.HUB)
        {
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
            Type.INITIAL => Instantiate(_healthReward, position, Quaternion.identity),
            Type.BOW => Instantiate(_bowReward, position, Quaternion.identity),
            Type.STRENGTH => Instantiate(_strengthReward, position, Quaternion.identity),
            Type.SPEED => Instantiate(_speedReward, position, Quaternion.identity),
            Type.SWORD => Instantiate(_swordReward, position, Quaternion.identity),
            Type.AXE => Instantiate(_axeReward, position, Quaternion.identity),
            Type.CIRCLE_ABILITY => Instantiate(_circleAbilityReward, position, Quaternion.identity),
            Type.AIRATTACK_ABILITY => Instantiate(_airAttackAbility, position, Quaternion.identity),
            Type.ICEPROJECTILE_ABILITY => Instantiate(_iceProjectileAbility, position, Quaternion.identity),
            Type.SHIELD_ABILITY => Instantiate(_shieldAbility, position, Quaternion.identity),
            _ => _rewardSpawned
        };
        _isControlEnabled = false;
    }

    public void StartRoom(Type roomType)
    {
        //set up the symbols for the next rooms
        if (LevelManager.Instance.GetRoomsTraversed()+1 != LevelManager.Instance.roomsBeforeBoss)
        {
            var toRemove = _possibleSymbols.Find(x => x.type == Type.BOSS);
            _possibleSymbols.Remove(toRemove);
            if (roomType is Type.INITIAL or Type.BOSS)
            {
                foreach (var d in _doors)
                {
                    d.SetSymbol(_possibleSymbols.Find((x) => x.type==Type.HUB));
                }
            }
            else
            {
                if (roomType == Type.HUB)
                {
                    toRemove = _possibleSymbols.Find(x => x.type == Type.SWORD);
                    _possibleSymbols.Remove(toRemove);
                    toRemove = _possibleSymbols.Find(x => x.type == Type.BOW);
                    _possibleSymbols.Remove(toRemove);
                    toRemove = _possibleSymbols.Find(x => x.type == Type.AXE);
                    _possibleSymbols.Remove(toRemove);
                    toRemove = _possibleSymbols.Find(x => x.type == Type.CIRCLE_ABILITY);
                    _possibleSymbols.Remove(toRemove);
                    toRemove = _possibleSymbols.Find(x => x.type == Type.ICEPROJECTILE_ABILITY);
                    _possibleSymbols.Remove(toRemove);
                    toRemove = _possibleSymbols.Find(x => x.type == Type.AIRATTACK_ABILITY);
                    _possibleSymbols.Remove(toRemove);
                    toRemove = _possibleSymbols.Find(x => x.type == Type.SHIELD_ABILITY);
                    _possibleSymbols.Remove(toRemove);
                }
                else
                {
                    if (Player.Instance.checkBowWeapon() || roomType == Type.BOW)
                    {
                        toRemove = _possibleSymbols.Find(x => x.type == Type.BOW);
                        _possibleSymbols.Remove(toRemove);
                    }
                    if (Player.Instance.checkSwordWeapon() || roomType == Type.SWORD)
                    {
                        toRemove = _possibleSymbols.Find(x => x.type == Type.SWORD);
                        _possibleSymbols.Remove(toRemove);
                    }
                    if (Player.Instance.checkAxeWeapon() || roomType == Type.AXE)
                    {
                        toRemove = _possibleSymbols.Find(x => x.type == Type.AXE);
                        _possibleSymbols.Remove(toRemove);
                    }
                    if (Player.Instance.GetComponentInChildren<CircleAbility>())
                    {
                        toRemove = _possibleSymbols.Find(x => x.type == Type.CIRCLE_ABILITY);
                        _possibleSymbols.Remove(toRemove);
                    }
                    if (Player.Instance.GetComponentInChildren<IceProjectileAbility>())
                    {
                        toRemove = _possibleSymbols.Find(x => x.type == Type.ICEPROJECTILE_ABILITY);
                        _possibleSymbols.Remove(toRemove);
                    }
                    if (Player.Instance.GetComponentInChildren<AirAttackAbility>())
                    {
                        toRemove = _possibleSymbols.Find(x => x.type == Type.AIRATTACK_ABILITY);
                        _possibleSymbols.Remove(toRemove);
                    }
                    if (Player.Instance.GetComponentInChildren<ShieldAbility>())
                    {
                        toRemove = _possibleSymbols.Find(x => x.type == Type.SHIELD_ABILITY);
                        _possibleSymbols.Remove(toRemove);
                    }
                }
            
                foreach (var d in _doors)
                {
                    var i = Random.Range(0, _possibleSymbols.Count);
                    d.SetSymbol(_possibleSymbols[i]);
                    _possibleSymbols.RemoveAt(i);
                }
            }
            
        }
        else
        {
            foreach (var d in _doors)
            {
                d.SetSymbol(_possibleSymbols.Find((x) => x.type==Type.BOSS));
            }
        }

        _roomType = roomType;
        if (_roomType == Type.RANDOM)
        {
            List<Type> randomSymbols = new List<Type>();
            randomSymbols.Add(Type.SPEED);
            randomSymbols.Add(Type.STRENGTH);
            _roomType = randomSymbols[Random.Range (1,(randomSymbols.Count - 1))];
        }
        switch (_roomType)
        {
            case Type.INITIAL:
                _numOfEnemies = 1;
                break;
            case Type.HUB:
                _numOfEnemies = 0;
                LevelManager.Instance.IncrementCurrentLevel();
                break;
            //Follower types do same thing at first
            case Type.BOW:
            case Type.SPEED:
            case Type.SWORD:
            case Type.STRENGTH:
            case Type.AXE:
            case Type.CIRCLE_ABILITY:
            case Type.ICEPROJECTILE_ABILITY:
            case Type.AIRATTACK_ABILITY:
            case Type.SHIELD_ABILITY:
                if (CompareTag("SmallRoom") && (LevelManager.Instance.GetCurrentLevel()) == 1)
                {
                    _numOfEnemies = Random.Range(10, 15);
                }
                else if (CompareTag("SmallRoom") && (LevelManager.Instance.GetCurrentLevel()) == 2)
                {
                    _numOfEnemies = Random.Range(15, 20);
                }
                else if (CompareTag("SmallRoom") && (LevelManager.Instance.GetCurrentLevel()) == 3)
                {
                    _numOfEnemies = Random.Range(20, 25);
                }
                else if (CompareTag("BigRoom") && (LevelManager.Instance.GetCurrentLevel()) == 1)
                {
                    _numOfEnemies = Random.Range(20, 25);
                }
                else if (CompareTag("BigRoom") && (LevelManager.Instance.GetCurrentLevel()) == 2)
                {
                    _numOfEnemies = Random.Range(30, 35);
                }
                else if (CompareTag("BigRoom") && (LevelManager.Instance.GetCurrentLevel()) == 3)
                {
                    _numOfEnemies = Random.Range(40, 50);
                }
                break;
            case Type.BOSS:
                _numOfEnemies = Random.Range(10, 15);
                StartCoroutine(SpawnBoss());
                break;
        }
        
        if (_roomType == Type.BOSS)
        {
            ModifyNumOfEnemies(1);
            StartCoroutine(SpawnEnemies(_numOfEnemies-1));
        }
        else
        {
            StartCoroutine(SpawnEnemies(_numOfEnemies));
        }
        UIController.Instance.SetEnemyCounter(_numOfEnemies);
    }

    private IEnumerator SpawnEnemies(int number)
    {
        int spiderCounter = 0;
        int spiderMax = (int) (number * spiderPercentage);
        /*print("spiders: " + spiderMax);
        print("skeletons" + (_numOfEnemies-spiderMax));*/
        
        //while (_availablePlaces.Count!=0) // uncomment to infinitely spawn enemies until no places are left
        for (int i = 0; i < number; i++) // uncomment to spawn a fixed amount of enemies
        {
            yield return new WaitForSeconds(_spawnTime);
            EnemyController type = _possibleEnemyType[Random.Range(0, _possibleEnemyType.Length)];
            _enemies.Add(_enemySpawner.Spawn(type));
            /*
            if (type.GetType() == typeof(SpiderController) && spiderCounter<spiderMax)
            {
                spiderCounter++;
                _enemies.Add(_enemySpawner.Spawn(type));
            }
            else if(type.GetType() == typeof(SkeletonController))
            {
                _enemies.Add(_enemySpawner.Spawn(type));
            }
            else
            {
                i--;
            }
            */
        }
    }

    private IEnumerator SpawnBoss()
    {
        if (numOfCrystals != 0)
        {
            for (int i = 0; i < numOfCrystals; i++)
            {
                yield return new WaitForSeconds(_spawnTime);
                var crystal = _enemySpawner.Spawn(possibleCrystalType[Random.Range(0, possibleCrystalType.Length)]);
                crystals.Add(crystal);
            }
        }
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
    
    public void DestroyAllFireballs()
    {
        _projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var t in _projectiles)
        {
            Destroy(t.gameObject);
        }
    }

    public void DestroyAllCrystals()
    {
        foreach (EnemyController c in crystals)
        {
            Destroy(c.gameObject);
        }
    }

    public int GetNumOfEnemies()
    {
        return _numOfEnemies;
    }

    public void ModifyNumOfEnemies(int delta)
    {
        _numOfEnemies += delta;
    }
    
    public void InstantiatePotion()
    {
        var position = spawnPointPotion.position;
        Instantiate(_healthReward, position, Quaternion.identity);
        Debug.Log("Potion spawned.");
    }

    public void AddEnemyToList(EnemyController enemy)
    {
        _enemies.Add(enemy);
    }
}
