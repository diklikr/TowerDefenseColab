using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int initialEnemies = 3;
    public int enemiesPerWaveIncrease = 2;
    public int totalWaves = 5;
    public SceneManage sceneManage;

    [Header("Tiempos")]
    public float intervaloSpawn = 0.5f;      //segundos entre enemigo y enemigo
    public float respiroEntreOleadas = 3f;   //tiempo de descanso tras limpiar una oleada

    [SerializeField]
    private int currentWave = 0;

    void Start()
    {
        EnemyHP.enemigosVivos = 0;   //limpia el contador al empezar la escena

        if (sceneManage == null)
        {
            sceneManage = FindAnyObjectByType<SceneManage>();
        }
        StartCoroutine(HandleWaves());
    }

    private IEnumerator HandleWaves()
    {
        while (currentWave < totalWaves)
        {
            int enemiesToSpawn = initialEnemies + (currentWave * enemiesPerWaveIncrease);

            //Spawnea todos los enemigos de la oleada
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(intervaloSpawn);
            }
            yield return null;

            //Espera a que el jugador limpie la oleada
            while (EnemyHP.enemigosVivos > 0)
            {
                //yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(respiroEntreOleadas);
            }

            currentWave++;

            //Respiro para construir muros antes de la siguiente
            /*if (currentWave < totalWaves)
            {
                yield return new WaitForSeconds(respiroEntreOleadas);
            }*/
        }

        if (sceneManage != null)
        {
            sceneManage.Win();
        }
    }

    //Instancia un enemigo en un punto de spawn aleatorio
    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 posicion = spawnPoints[randomIndex].position;

        //Ajusta la posición al NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(posicion, out hit, 5f, NavMesh.AllAreas))
        {
            posicion = hit.position;
        }

        Instantiate(enemyPrefab, posicion, spawnPoints[randomIndex].rotation);
    }
}