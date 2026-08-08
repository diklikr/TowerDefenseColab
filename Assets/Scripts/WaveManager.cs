using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int initialEnemies = 3;
    public int enemiesPerWaveIncrease = 2;
    public int totalWaves = 5;
    public float cooldownBetweenWaves = 10f;

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(HandleWaves());
    }

    // This handles the wave progression and cooldown times
    private IEnumerator HandleWaves()
    {
        while (currentWave < totalWaves)
        {
            int enemiesToSpawn = initialEnemies + (currentWave * enemiesPerWaveIncrease);

            // This spawns the required number of enemies for the current wave
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }

            currentWave++;

            if (currentWave < totalWaves)
            {
                yield return new WaitForSeconds(cooldownBetweenWaves);
            }
        }
    }

    // This instantiates an enemy at a random spawn point
    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
    }
}