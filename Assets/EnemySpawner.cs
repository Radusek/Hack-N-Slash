using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private int maxEnemiesSpawnedCount = 5;
    private int currentEnemiesSpawnedCount = 0;

    [SerializeField]
    private float areaRadius = 8f;

    [SerializeField]
    private float minPlayerDistance = 50f;

    private Transform playerTransform;

    private GameObject[] enemiesPool;
    private List<int> indexesOfDeadEnemies;

    private void Awake()
    {
        enemiesPool = new GameObject[maxEnemiesSpawnedCount];
        for (int i = 0; i < maxEnemiesSpawnedCount; i++)
            enemiesPool[i] = GetEnemy();
        
        indexesOfDeadEnemies = new List<int>();
    }

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        StartCoroutine(SpawningCoroutine());
    }

    private GameObject GetEnemy()
    {
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector3 position = transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * areaRadius;
        position.x += randomOffset.x;
        position.z += randomOffset.y;

        GameObject go = Instantiate(prefab, position, Quaternion.identity);
        go.GetComponent<EnemyController>().SetMobArea(transform.position, areaRadius);
        return go;
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
            if (enemiesPool[i] != null)
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
            indexesOfDeadEnemies.RemoveAt(0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}
