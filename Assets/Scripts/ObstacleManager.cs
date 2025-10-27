using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] carPrefabs;
    public GameObject[] zombiePrefabs;
    public GameObject[] manholePrefabs;
    
    public float spawnInterval = 3f;
    public Vector2 spawnArea = new Vector2(10f, 5f);
    public float spawnZPosition = 0f;
    
    [Header("Spawn Rates")]
    [Range(0, 100)] public int carSpawnChance = 30;
    [Range(0, 100)] public int zombieSpawnChance = 25;
    [Range(0, 100)] public int manholeSpawnChance = 20;

    private float nextSpawnTime = 0f;
    private List<Obstacle> activeObstacles = new List<Obstacle>();

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
            return;

        // Spawn obstacles on timer
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomObstacle();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Clean up destroyed obstacles
        activeObstacles.RemoveAll(obs => obs == null);
    }

    void SpawnRandomObstacle()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // Roll for obstacle type
        int roll = Random.Range(0, 100);
        
        if (roll < carSpawnChance && carPrefabs != null && carPrefabs.Length > 0)
        {
            SpawnCar(spawnPosition);
        }
        else if (roll < carSpawnChance + zombieSpawnChance && zombiePrefabs != null && zombiePrefabs.Length > 0)
        {
            SpawnZombie(spawnPosition);
        }
        else if (roll < carSpawnChance + zombieSpawnChance + manholeSpawnChance && manholePrefabs != null && manholePrefabs.Length > 0)
        {
            SpawnManhole(spawnPosition);
        }
    }

    public void SpawnCar(Vector3 position)
    {
        GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];
        GameObject car = Instantiate(carPrefab, position, Quaternion.identity);
        
        CarObstacle carScript = car.GetComponent<CarObstacle>();
        if (carScript != null)
        {
            activeObstacles.Add(carScript);
            
            // Randomize car properties
            carScript.SetSpeed(Random.Range(3f, 8f));
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), 0).normalized;
            carScript.SetDirection(randomDir);
        }
    }

    public void SpawnZombie(Vector3 position)
    {
        GameObject zombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
        GameObject zombie = Instantiate(zombiePrefab, position, Quaternion.identity);
        
        ZombieEnemy zombieScript = zombie.GetComponent<ZombieEnemy>();
        if (zombieScript != null)
        {
            activeObstacles.Add(zombieScript);
        }
    }

    public void SpawnManhole(Vector3 position)
    {
        GameObject manholePrefab = manholePrefabs[Random.Range(0, manholePrefabs.Length)];
        GameObject manhole = Instantiate(manholePrefab, position, Quaternion.identity);
        
        ManholeTrap manholeScript = manhole.GetComponent<ManholeTrap>();
        if (manholeScript != null)
        {
            activeObstacles.Add(manholeScript);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            transform.position.x + Random.Range(-spawnArea.x, spawnArea.x),
            transform.position.y + Random.Range(-spawnArea.y, spawnArea.y),
            spawnZPosition
        );
    }

    // Clear all active obstacles
    public void ClearAllObstacles()
    {
        foreach (Obstacle obstacle in activeObstacles)
        {
            if (obstacle != null)
            {
                obstacle.OnDestroy();
            }
        }
        activeObstacles.Clear();
    }

    // Set spawn rate
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }

    // Get active obstacles count
    public int GetActiveObstacleCount()
    {
        return activeObstacles.Count;
    }
}
