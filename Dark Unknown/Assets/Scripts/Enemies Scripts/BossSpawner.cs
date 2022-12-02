using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
    public class BossSpawner: MonoBehaviour
    {
        public EnemyController SpawnBoss(EnemyController bossPre, Transform bossSpawnPoint)
        {
            Vector3 bossPosition = bossSpawnPoint.position;
            EnemyController boss = (EnemyController) Instantiate(bossPre, bossPosition, Quaternion.identity);
            return boss;
        }
    }