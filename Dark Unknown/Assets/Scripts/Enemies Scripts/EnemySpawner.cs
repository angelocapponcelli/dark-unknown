using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//namespace Enemies_Scripts
//{
    public class EnemySpawner : MonoBehaviour
    {
        private List<Vector3> _availablePlaces;

        [SerializeField] public Tilemap tileMap;

        // Start is called before the first frame update
        private void Start()
        {
            _availablePlaces = new List<Vector3>();

            for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
            {
                for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
                {
                    var localPlace = (new Vector3Int(n, p, 0));
                    var place = tileMap.CellToWorld(localPlace);
                    if (tileMap.HasTile(localPlace))
                    {
                        _availablePlaces.Add(place);
                    }
                }
            }
        
            /* Replaced in room logic
            if (enemyPrefab != null)
            {
               // StartCoroutine(nameof(SpawnEnemy));
            }*/
        }

        // In this configuration, no 2 enemies can spawn on the same tile
        public EnemyController Spawn(EnemyController enemyType)
        {
            var randomPlace = _availablePlaces[Random.Range(0, _availablePlaces.Count)];
            EnemyController enemy = (EnemyController) Instantiate(enemyType, randomPlace, Quaternion.identity);
            _availablePlaces.Remove(randomPlace);
            return enemy;
        }

        /* Replaced in room logic
        // In this configuration, no 2 enemies can spawn on the same tile
        private IEnumerator SpawnEnemy()
        {
            //while (_availablePlaces.Count!=0) // uncomment to infinitely spawn enemies until no places are left
            for (int i = 0; i < totalEnemies; i++) // uncomment to spawn a fixed amount of enemies
            {
                yield return new WaitForSeconds(spawnTime);
                var randomPlace = _availablePlaces[Random.Range(0, _availablePlaces.Count)];
                Instantiate(enemyPrefab, randomPlace, Quaternion.identity);
                _availablePlaces.Remove(randomPlace);
                /*for (int i = 0; i < enemyPerSpawn; i++)
                {
                    Instantiate(enemyPrefab, _availablePlaces[Random.Range(0, _availablePlaces.Count)], Quaternion.identity);
                }/
            }
        }*/
    }
//}