using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPointsEnemy;
    [SerializeField] private Transform[] _spawnPointsPlayer;
    [SerializeField] private EnemyController[] _possibleEnemy;
    [SerializeField] private int _numOfEnememy;
    [SerializeField] private Door[] _doors;
    private List<EnemyController> _enemies;

    private void Start()
    {
        //Player spawn getting a random point from possible player spawn points
        Player.Instance.SetPosition(_spawnPointsPlayer[Random.Range(0, _spawnPointsPlayer.Length)].position);

        //Enemy spawn getting a random point from possible enemy spawn points
        for (int i=0; i < _numOfEnememy; i++)
        {
            EnemyController enemy = Instantiate(_possibleEnemy[Random.Range(0, _possibleEnemy.Length)], _spawnPointsEnemy[Random.Range(0, _spawnPointsEnemy.Length)].position, Quaternion.identity) as EnemyController;
            _enemies.Add(enemy);
           // _enemies.Add(Instantiate(_possibleEnemy[Random.Range(0, _possibleEnemy.Length)], _spawnPointsEnemy[Random.Range(0, _spawnPointsEnemy.Length)].position, Quaternion.identity).GetComponent<EnemyController>());
            //salvare una lista di Enemy (classe dove c'è isDeath dell'enemy)
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        //TODO: aggiustare
        
        bool allDead = false;
        for(int i=0; i < _enemies.Count; i++)
        {
            if (_enemies[i].IsDead() == false)
                return;
            allDead = true;
        }
        if (allDead)
        {
            foreach(Door d in _doors)
            {
                d.Open();
            }
            //qualcosa succede dopo che tutti i nemici sono morti
        }
        
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Door d in _doors)
            {
                d.Open();
            }
        }*/
        
    }
}
