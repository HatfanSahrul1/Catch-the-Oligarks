using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private GameObject[] platformPrefabs; // Your 5 different platform prefabs
    [SerializeField] private Transform startPlatformPosition; // Empty object marking start position
    [SerializeField] private int initialPlatformCount = 5; // How many platforms to spawn at start
    [SerializeField] private int maxPoolSize = 10; // Maximum number of platforms in the pool
    [SerializeField] private float playerThreshold = 5f; // Distance from player when new platforms spawn
    [SerializeField] private float recycleThreshold = 20f; // Distance behind player when platforms are recycled
    [SerializeField] private Transform player; // Reference to player

    // Object pool collections
    private List<GameObject> activePlatforms = new List<GameObject>();
    private List<GameObject>[] platformPools; // Array of pools, one for each prefab type
    private List<int> platformSequence = new List<int>(); // Sequence of platform types to spawn

    // Track the position where the next platform should be placed
    private Transform nextSpawnPoint;

    private void Awake()
    {
        // Initialize our pools array
        platformPools = new List<GameObject>[platformPrefabs.Length];
        for (int i = 0; i < platformPrefabs.Length; i++)
        {
            platformPools[i] = new List<GameObject>();
        }

        // Set initial spawn point
        nextSpawnPoint = startPlatformPosition;

        // Generate initial platform sequence
        GeneratePlatformSequence();
    }

    private void Start()
    {
        // Spawn initial platforms
        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnNextPlatform();
        }
    }

    private void Update()
    {
        // Check if we need to spawn more platforms based on player position
        if (activePlatforms.Count > 0)
        {
            GameObject lastPlatform = activePlatforms[activePlatforms.Count - 1];
            float distanceToLastPlatform = Vector2.Distance(
                new Vector2(player.position.x, 0),
                new Vector2(lastPlatform.transform.position.x, 0)
            );

            if (distanceToLastPlatform < playerThreshold)
            {
                SpawnNextPlatform();
            }
        }

        // Check if we need to recycle platforms that are behind the player
        RecycleOldPlatforms();
    }

    private void GeneratePlatformSequence()
    {
        // Clear existing sequence
        platformSequence.Clear();

        // Create a sequence with each platform type
        for (int i = 0; i < platformPrefabs.Length; i++)
        {
            platformSequence.Add(i);
        }

        // Shuffle the sequence
        for (int i = 0; i < platformSequence.Count; i++)
        {
            int temp = platformSequence[i];
            int randomIndex = Random.Range(i, platformSequence.Count);
            platformSequence[i] = platformSequence[randomIndex];
            platformSequence[randomIndex] = temp;
        }
    }

    private void SpawnNextPlatform()
    {
        // Get the next platform type from our sequence
        int platformIndex = GetNextPlatformIndex();

        // Get a platform from the pool or create a new one
        GameObject platform = GetPlatformFromPool(platformIndex);

        // Position the platform at the next spawn point
        platform.transform.position = nextSpawnPoint.position;

        // Add to active platforms
        activePlatforms.Add(platform);

        // Find the spawn point for the next platform
        Transform newSpawnPoint = FindSpawnPointInPlatform(platform);
        if (newSpawnPoint != null)
        {
            nextSpawnPoint = newSpawnPoint;
        }
        else
        {
            Debug.LogWarning("No spawn point found in platform: " + platform.name);
        }
    }

    private int GetNextPlatformIndex()
    {
        // If we've used all platforms in our sequence, generate a new one
        if (platformSequence.Count == 0)
        {
            GeneratePlatformSequence();
        }

        // Get the next platform type and remove it from the sequence
        int platformIndex = platformSequence[0];
        platformSequence.RemoveAt(0);

        return platformIndex;
    }

    private GameObject GetPlatformFromPool(int platformIndex)
    {
        List<GameObject> specificPool = platformPools[platformIndex];

        // Check if there's an inactive platform in the pool
        foreach (GameObject platformObj in specificPool)
        {
            if (!platformObj.activeInHierarchy)
            {
                platformObj.SetActive(true);
                return platformObj;
            }
        }

        // If no inactive platform is found, create a new one if pool isn't full
        if (specificPool.Count < maxPoolSize)
        {
            GameObject newPlatform = Instantiate(platformPrefabs[platformIndex]);
            specificPool.Add(newPlatform);
            return newPlatform;
        }

        // If pool is full, find the oldest platform from our active list that matches this type
        for (int i = 0; i < activePlatforms.Count; i++)
        {
            // Find the oldest active platform of this specific type
            if (IsPlatformOfType(activePlatforms[i], platformIndex))
            {
                GameObject oldestPlatform = activePlatforms[i];
                activePlatforms.RemoveAt(i);
                return oldestPlatform;
            }
        }

        // Fallback - create a new one anyway
        GameObject fallbackPlatform = Instantiate(platformPrefabs[platformIndex]);
        specificPool.Add(fallbackPlatform);
        return fallbackPlatform;
    }

    private bool IsPlatformOfType(GameObject platform, int typeIndex)
    {
        // Compare with the original prefab to determine platform type
        // This is a simple implementation - you might need a more robust system
        string platformName = platformPrefabs[typeIndex].name;
        return platform.name.StartsWith(platformName);
    }

    private Transform FindSpawnPointInPlatform(GameObject platform)
    {
        // Find the child object that marks where the next platform should connect
        Transform spawnPoint = platform.transform.Find("NextPlatformPosition");

        if (spawnPoint == null)
        {
            // Look through all children for the spawn point
            foreach (Transform child in platform.transform)
            {
                if (child.name == "NextPlatformPosition" || child.CompareTag("NextPlatformPosition"))
                {
                    spawnPoint = child;
                    break;
                }
            }
        }

        return spawnPoint;
    }

    private void RecycleOldPlatforms()
    {
        // Only recycle platforms if we have more than the minimum required
        if (activePlatforms.Count <= 2)
            return;

        // Check from the beginning of our active platforms list (oldest platforms)
        for (int i = 0; i < activePlatforms.Count; i++)
        {
            GameObject platform = activePlatforms[i];

            // Calculate if this platform is far enough behind the player to recycle
            float distanceBehindPlayer = player.position.x - (platform.transform.position.x + 30f);

            // Only recycle if it's completely behind the player by our threshold amount
            if (distanceBehindPlayer > recycleThreshold)
            {
                // Debug log to help troubleshoot
                Debug.Log($"Recycling platform {platform.name} at position {platform.transform.position.x}, player at {player.position.x}, distance: {distanceBehindPlayer}");

                // Remove from active list
                activePlatforms.RemoveAt(i);
                i--; // Adjust index since we removed an item

                // Deactivate the platform (returning it to the pool)
                platform.SetActive(false);
            }
        }
    }

    // Optional: Add this method to help debug issues
    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(10, 10, 300, 20), $"Active Platforms: {activePlatforms.Count}");
    //     GUI.Label(new Rect(10, 30, 300, 20), $"Player Position: {player.position.x:F2}");

    //     if (activePlatforms.Count > 0)
    //     {
    //         GUI.Label(new Rect(10, 50, 300, 20), $"First Platform: {activePlatforms[0].transform.position.x:F2}");
    //         GUI.Label(new Rect(10, 70, 300, 20), $"Last Platform: {activePlatforms[activePlatforms.Count - 1].transform.position.x:F2}");
    //     }
    // }
}