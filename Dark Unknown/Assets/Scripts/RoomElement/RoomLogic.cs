using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnSpointsEnemy;
    [SerializeField] private Transform[] _spawnPointsPlayer;
    [SerializeField] private GameObject[] _possibleEnemy;
    [SerializeField] private int _numOfEnememy;
    [SerializeField] private Door[] _doors;
    private void Start()
    {
        Player.Instance.SetPosition(_spawnPointsPlayer[Random.Range(0, _spawnPointsPlayer.Length)].position);
        for(int i=0; i < _numOfEnememy; i++)
        {
            Instantiate(_possibleEnemy[Random.Range(0, _possibleEnemy.Length)], _spawnSpointsEnemy[Random.Range(0, _spawnSpointsEnemy.Length)].position, Quaternion.identity);
            //salvare una lista di Enemy (classe dove c'è isDeath dell'enemy)
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        //TODO: aggiustare
        /*
        bool allDeath = false;
        for(int i=0; i < _enemy; i++)
        {
            if (_enemy[i].isDeath == false)
                return;
            allDeath = true;
        }
        if (allDeath)
        {
            foreach(Door d in _doors)
            {
                d.Open();
            }
            //qualcosa succede dopo che tutti i nemici sono morti
        }
        */
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Door d in _doors)
            {
                d.Open();
            }
        }
        
    }
}
