

using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabsToSpawn; // List of prefabs to spawn.
    public Transform spawnPoint;       // The spawn location.
    public float initialSpawnDelay = 2f; // Initial delay before spawning starts.

    [Header("Auto Start")]
    public bool startOnAwake = false;  // Whether to start spawning on Awake.

    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (startOnAwake)
        {
            StartSpawning();
        }
    }

    /// <summary>
    /// Starts the spawning process.
    /// </summary>
    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    /// <summary>
    /// Stops the spawning process.
    /// </summary>
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        // Initial delay before spawning starts.
        yield return new WaitForSeconds(initialSpawnDelay);

        // Spawn all prefabs without delay in between.
        foreach (GameObject prefab in prefabsToSpawn)
        {
            if (prefab != null)
            {
                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}