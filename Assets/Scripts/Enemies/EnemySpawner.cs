using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private PoolType[] enemyTypes;

    [SerializeField]
    private int maxEnemiesSpawnedCount = 5;
    private int currentEnemiesSpawnedCount = 0;

    [SerializeField]
    private float areaRadius = 8f;

    [SerializeField]
    private float minPlayerDistance = 50f;

    private Transform playerTransform;

    private IRecyclable[] enemiesPool;
    private List<int> indexesOfDeadEnemies;


    private void Start()
    {
        enemiesPool = new IRecyclable[maxEnemiesSpawnedCount];
        for (int i = 0; i < Random.Range(1, maxEnemiesSpawnedCount + 1); i++)
            enemiesPool[i] = GetEnemy();

        indexesOfDeadEnemies = new List<int>();

        playerTransform = FindObjectOfType<PlayerController>().transform;
        StartCoroutine(SpawningCoroutine());
    }

    private IRecyclable GetEnemy()
    {
        int enemyTypeIndex = Random.Range(0, enemyTypes.Length);
        int randomizedIndex = enemyTypeIndex;
        PoolType enemyType = enemyTypes[enemyTypeIndex];
        do
        {
            if (ObjectPoolManager.Instance.PoolCount(enemyType) <= 0)
            {
                enemyTypeIndex++;
                enemyTypeIndex %= enemyTypes.Length;
                enemyType = enemyTypes[enemyTypeIndex];
                continue;
            }

            IRecyclable enemy = ObjectPoolManager.Instance.DequeueObject(enemyType);
            enemy.SetInitialEnemyValues(GetSpawningPosition(), transform.position, areaRadius);

            return enemy;
        } while (enemyTypeIndex != randomizedIndex);
        return null;
    }

    private Vector3 GetSpawningPosition()
    {
        Vector3 position = transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * areaRadius;
        position.x += randomOffset.x;
        position.z += randomOffset.y;
        return position;
    }

    private IEnumerator SpawningCoroutine()
    {
        var delay = new WaitForSeconds(30f);
        while(true)
        {
            UpdateEnemiesPool();

            if (currentEnemiesSpawnedCount < maxEnemiesSpawnedCount)
            {
                if ((playerTransform.position - transform.position).sqrMagnitude > minPlayerDistance * minPlayerDistance)
                {
                    SpawnEnemies();
                }
            }
            yield return delay;
        }
    }

    private void UpdateEnemiesPool()
    {
        currentEnemiesSpawnedCount = 0;
        indexesOfDeadEnemies.Clear();
        for (int i = 0; i < enemiesPool.Length; i++)
        {
            if (enemiesPool[i] != null && enemiesPool[i].IsActive())
                currentEnemiesSpawnedCount++;
            else
                indexesOfDeadEnemies.Add(i);
        }
    }

    private void SpawnEnemies()
    {
        int enemiesToSpawn = Random.Range(1, indexesOfDeadEnemies.Count);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            enemiesPool[indexesOfDeadEnemies[0]] = GetEnemy();
            if (enemiesPool[indexesOfDeadEnemies[0]] == null)
                return;

            indexesOfDeadEnemies.RemoveAt(0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}
