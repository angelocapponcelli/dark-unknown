using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    [Header ("Player spawner")]
    [SerializeField] private Transform[] _spawnPointsPlayer;

    [Header("Enemy spawner")]
    [SerializeField] private EnemyController[] _possibleEnemyType;
    [SerializeField] private int _numOfEnememy;
    [SerializeField] private float _spawnTime = 1.0f;
    private List<EnemyController> _enemies = new List<EnemyController>();
    private EnemySpawner _enemySpawner;

    [Header("")]
    [SerializeField] private Door[] _doors;
    [SerializeField] private Transform _spawnPointReward;
    //[SerializeField] private Dictionary<Type, Reward> _rewards;
    private Reward _rewardSpawned;

    [Header("Rewards")]
    [SerializeField] private HealthReward _healthReward;
    [SerializeField] private SpeedReward _speedReward;
    [SerializeField] private BowReward _bowReward;
    [SerializeField] private StrengthReward _streagthReward;

    public enum Type {INITIAL, HEALTH, BOW, SPEED, HARD, EASY};
    private Type _roomType;
    private bool _isControllEnabled = true;

    private void Start()
    {
        //Player spawn getting a random point from possible player spawn points
        Player.Instance.SetPosition(_spawnPointsPlayer[Random.Range(0, _spawnPointsPlayer.Length)].position);

        //initialize _enemySpawner and call the coroutine which call the enemySpawner method to spawn all enemies 
        _enemySpawner = GetComponent<EnemySpawner>();
        //_rewardSpawned = Instantiate(_rewardSpawned, _spawnPointReward.position, Quaternion.identity);
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
                if (allDead || _numOfEnememy==0)
                {
                    foreach (Door d in _doors)
                    {
                        d.Open();
                    }
                    switch (_roomType)
                    {
                        case Type.INITIAL:
                            _numOfEnememy = 0;
                            break;
                        case Type.HEALTH:
                            _rewardSpawned = Instantiate(_healthReward, _spawnPointReward.position, Quaternion.identity);
                            //_rewardSpawned.AddComponent<HealthReward>();
                            break;
                        case Type.BOW:
                            //_rewardSpawned.AddComponent<BowReward>();
                            _rewardSpawned = Instantiate(_bowReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                        case Type.EASY:
                            //TODO provvisorio per testare ma sbagliato
                            _rewardSpawned = Instantiate(_speedReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                        case Type.HARD:
                            //TODO provvisorio per testare ma sbagliato
                            _rewardSpawned = Instantiate(_streagthReward, _spawnPointReward.position, Quaternion.identity);
                            break;
                    }
                    _isControllEnabled = false;
                }
            }
        }     
    }

    public void StartRoom(Type roomType)
    {
        _roomType = roomType;
        switch(roomType)
        {
            case Type.INITIAL:
                _numOfEnememy = 0;
                break;
            case Type.HEALTH:
                break;
            case Type.BOW:
                break;
            case Type.EASY:
                _numOfEnememy = 2;
                break;
            case Type.HARD:
                _numOfEnememy = 10;
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

    public void DestroyAllEnemies()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            Destroy(_enemies[i].gameObject);
        }
    }
}
