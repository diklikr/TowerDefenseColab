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
    public SceneManage sceneManage;

    private int currentWave = 0;

    void Start()
    {
        if (sceneManage == null)
        {
            sceneManage = FindObjectOfType<SceneManage>();
        }
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

        // Wait until all remaining enemies are destroyed to trigger victory
        while (FindObjectsOfType<EnemyHP>().Length > 0)
        {
            yield return new WaitForSeconds(1.0f);
        }

        if (sceneManage != null)
        {
            sceneManage.Win();
        }
        else
        {
            Debug.LogError("SceneManage no asignado ni encontrado en la escena. No se puede cargar pantalla de victoria.");
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