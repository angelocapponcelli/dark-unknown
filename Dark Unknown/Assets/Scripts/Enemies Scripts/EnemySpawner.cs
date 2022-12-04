using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    private List<Vector3> _availablePlaces;

    [SerializeField] public Tilemap tileMap;
    [SerializeField] public float spawnTime;
    [SerializeField] public EnemyController enemyPrefab;

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
        // comment this next while building
        //StartCoroutine(SpawnEnemy());
    }

    // In this configuration, no 2 enemies can spawn on the same tile
    public EnemyController Spawn(EnemyController enemyType)
    {
        var randomPlace = _availablePlaces[Random.Range(0, _availablePlaces.Count)];
        EnemyController enemy = (EnemyController) Instantiate(enemyType, randomPlace, Quaternion.identity);
        _availablePlaces.Remove(randomPlace);
        return enemy;
    }
    
    public static EnemyController SpawnBoss(EnemyController bossPre, Transform bossSpawnPoint)
    {
        Vector3 bossPosition = bossSpawnPoint.position;
        EnemyController boss = (EnemyController) Instantiate(bossPre, bossPosition, Quaternion.identity);
        return boss;
    }

    // Replaced in room logic
    // In this configuration, no 2 enemies can spawn on the same tile
    /*private IEnumerator SpawnEnemy()
    {
        while (_availablePlaces.Count!=0) // uncomment to infinitely spawn enemies until no places are left
        //for (int i = 0; i < totalEnemies; i++) // uncomment to spawn a fixed amount of enemies
        {
            yield return new WaitForSeconds(spawnTime);
            var randomPlace = _availablePlaces[Random.Range(0, _availablePlaces.Count)];
            Instantiate(enemyPrefab, randomPlace, Quaternion.identity);
            _availablePlaces.Remove(randomPlace);
        }
    }*/
}