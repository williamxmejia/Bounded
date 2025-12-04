using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

        [Header("Enemy Spawner Settings")]
        [Tooltip("Prefab to spawn")] public GameObject enemyPrefab;
        [Tooltip("Terrain GameObject")] public GameObject terrainObject;
        [Tooltip("Time in seconds between spawns")] public float spawnInterval = 5f;
        [Tooltip("Maximum number of enemies to spawn at a time")] public int maxSpawnCount = 10;

    private float timer = 0f;
    private int spawnedCount = 0;

    void Update()
    {
        if (enemyPrefab == null) return;
        if (terrainObject == null) return;
        if (maxSpawnCount > 0 && spawnedCount >= maxSpawnCount) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Terrain terrain = terrainObject.GetComponent<Terrain>();
            if (terrain != null)
            {
                Vector3 terrainPos = terrain.transform.position;
                float terrainWidth = terrain.terrainData.size.x;
                float terrainLength = terrain.terrainData.size.z;
                float randX = Random.Range(terrainPos.x, terrainPos.x + terrainWidth);
                float randZ = Random.Range(terrainPos.z, terrainPos.z + terrainLength);
                float randY = terrain.SampleHeight(new Vector3(randX, 0, randZ)) + terrainPos.y;
                Vector3 spawnPos = new Vector3(randX, randY, randZ);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnedCount++;
                timer = 0f;
            }
        }
    }
}
