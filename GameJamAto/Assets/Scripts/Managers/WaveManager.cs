using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemyData
{
    public GameObject EnemyPrefab;
    public int indexOfPath;

    public WaveEnemyData(GameObject e, int i) { EnemyPrefab = e;indexOfPath = i; }
}

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance;
    public static WaveManager Instance { get { return instance; } }

    private Path[] pathArray;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float[] enemyWeight; //Chances for the enemy to spawn
    private float sumWeight;

    private WaveEnemyData[] waveEnemy;
    [SerializeField] private float spawnDelay = 1.0f;
    private int enemyDeadThisWave = 0;

    [SerializeField] private int numberOfEnnemyPerWave = 5;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        sumWeight = 0;
        for (int i = 0; i < enemyWeight.Length; i++)
            sumWeight += enemyWeight[i];
    }

    private void Start()
    {
        pathArray = GameObject.FindObjectsOfType<Path>();
    }

    public void StartWave(int waveIndex)
    {
        enemyDeadThisWave = 0;
        InitializeWaveData(waveIndex);
        StartCoroutine(SpawnEnemies());
    }

    private void InitializeWaveData(int waveIndex)
    {
        waveEnemy = new WaveEnemyData[numberOfEnnemyPerWave * (waveIndex + 1) / 2];
        for(int i=0;i<waveEnemy.Length;i++)
        {
            float weight = Random.Range(0, sumWeight);
            int enemyIndex = 0;
            for(enemyIndex = 0; enemyIndex < enemyWeight.Length; enemyIndex++)
            {
                if (weight < enemyWeight[enemyIndex])
                    break;
                weight -= enemyWeight[enemyIndex];
            }
            waveEnemy[i] = new WaveEnemyData(enemyPrefabs[enemyIndex], Random.Range(0, pathArray.Length));
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int currentEnemy = 0;
        
        while(currentEnemy < waveEnemy.Length)
        {
            yield return new WaitForSeconds(spawnDelay);
            if (GameManager.Instance.CurrentGameState != GameState.Playing)
                break;
            GameObject newEnemy = Instantiate(waveEnemy[currentEnemy].EnemyPrefab, pathArray[waveEnemy[currentEnemy].indexOfPath].Points[0].position, Quaternion.identity);
            newEnemy.GetComponent<Ennemy>()._path = pathArray[waveEnemy[currentEnemy].indexOfPath];
            currentEnemy++;
        }
    }

    public void EnemyDies()
    {
        enemyDeadThisWave++;
        if(enemyDeadThisWave >= waveEnemy.Length)
        {
            GameManager.Instance.WaveFinished();
            enemyDeadThisWave = 0;
        }
    }

    //Récupérer les paths
    //Avoir les prefab des ennemis
    //Définir quel ennemi va spawner, ou et quand
    //Augmenter le nombre d'ennemi par wave
    //Définir le début d'une wave
    //Définir la fin d'une wave
}
