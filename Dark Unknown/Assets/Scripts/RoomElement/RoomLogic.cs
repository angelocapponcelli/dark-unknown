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

    public enum Type { INITIAL, HEALTH, WEAPONS, SPEED, HARD, EASY};
    private Type _roomType;

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
                foreach (Door d in _doors)
                {
                    d.Open();
                }
                //TODO qualcosa succede dopo che tutti i nemici sono morti es:Spwanare il powerUp della stanza (Cuoricino per la vita, una nuova arma)
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
            case Type.WEAPONS:
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
        Debug.Log("Distruggi");
        for (int i = 0; i < _enemies.Count; i++)
        {
            Destroy(_enemies[i].gameObject);
        }
    }
}
