using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    [Header("Road Settings")]
    public GameObject[] roadSegmentPrefabs;
    public GameObject player;
    
    public float roadSegmentLength = 20f;
    public float spawnDistanceAhead = 50f;
    public float despawnDistanceBehind = 30f;
    public int initialSegments = 3;
    
    [Header("Spawn Settings")]
    public bool spawnCars = true;
    public bool spawnZombies = true;
    public bool spawnManholes = true;
    public ObstacleManager obstacleManager;
    
    private List<GameObject> activeRoadSegments = new List<GameObject>();
    private float lastSpawnZ = 0f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Spawn initial road segments
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnRoadSegment(lastSpawnZ);
        }
    }

    void Update()
    {
        if (player == null) return;

        // Spawn new segments ahead of player
        if (player.transform.position.z >= lastSpawnZ - spawnDistanceAhead)
        {
            SpawnRoadSegment(lastSpawnZ);
        }

        // Despawn segments behind player
        DespawnOldSegments();
    }

    void SpawnRoadSegment(float zPosition)
    {
        if (roadSegmentPrefabs == null || roadSegmentPrefabs.Length == 0)
        {
            Debug.LogWarning("No road segment prefabs assigned!");
            return;
        }

        // Choose random road segment
        GameObject segmentPrefab = roadSegmentPrefabs[Random.Range(0, roadSegmentPrefabs.Length)];
        Vector3 spawnPosition = new Vector3(0, 0, zPosition);
        
        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity, transform);
        activeRoadSegments.Add(newSegment);
        
        lastSpawnZ += roadSegmentLength;

        // Spawn obstacles on the road
        if (obstacleManager != null)
        {
            SpawnObstaclesOnSegment(newSegment, zPosition);
        }
    }

    void SpawnObstaclesOnSegment(GameObject segment, float zPosition)
    {
        int obstacleCount = Random.Range(1, 4); // 1-3 obstacles per segment
        
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 obstaclePos = new Vector3(
                Random.Range(-5f, 5f), // Random X position
                0,
                zPosition + Random.Range(0, roadSegmentLength)
            );

            // Random obstacle type
            int typeRoll = Random.Range(0, 100);
            
            if (spawnCars && typeRoll < 40 && obstacleManager.carPrefabs != null && obstacleManager.carPrefabs.Length > 0)
            {
                obstacleManager.SpawnCar(obstaclePos);
            }
            else if (spawnZombies && typeRoll < 70 && obstacleManager.zombiePrefabs != null && obstacleManager.zombiePrefabs.Length > 0)
            {
                obstacleManager.SpawnZombie(obstaclePos);
            }
            else if (spawnManholes && obstacleManager.manholePrefabs != null && obstacleManager.manholePrefabs.Length > 0)
            {
                obstacleManager.SpawnManhole(obstaclePos);
            }
        }
    }

    void DespawnOldSegments()
    {
        if (player == null) return;

        List<GameObject> segmentsToRemove = new List<GameObject>();

        foreach (GameObject segment in activeRoadSegments)
        {
            if (segment == null)
            {
                segmentsToRemove.Add(segment);
                continue;
            }

            // Check if segment is behind player
            if (segment.transform.position.z < player.transform.position.z - despawnDistanceBehind)
            {
                Destroy(segment);
                segmentsToRemove.Add(segment);
            }
        }

        foreach (GameObject segment in segmentsToRemove)
        {
            activeRoadSegments.Remove(segment);
        }
    }

    // Reset road generator
    public void ResetRoad()
    {
        foreach (GameObject segment in activeRoadSegments)
        {
            if (segment != null)
                Destroy(segment);
        }
        activeRoadSegments.Clear();
        lastSpawnZ = 0f;
        
        // Respawn initial segments
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnRoadSegment(lastSpawnZ);
        }
    }
}
