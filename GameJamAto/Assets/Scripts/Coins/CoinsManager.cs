using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class CoinsManager : MonoBehaviour
{

    private static CoinsManager instance;
    public static CoinsManager Instance { get => instance; }
    [SerializeField]
    private GameObject _coin;

    private float _nextSpawn;
    [SerializeField]
    private float _minNextSpawn;
    [SerializeField]
    private float _maxNextSpawn;

    private float _timeSinceLastSpawn = 0;

    [SerializeField]
    private int _maxCoinsInGame=10;
    [SerializeField]
    private int currentCoinsCountInGame;


    [SerializeField]
    public List<CoinsSpawner> CoinsSpanwers;

    private bool playing = false;

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartGame()
    {
        _nextSpawn = PickNextSpawn();

        currentCoinsCountInGame = GameManager.Instance.CoinLeft;
        Tower[] towers = GameObject.FindObjectsOfType<Tower>();
        if (towers != null)
        {
            for (int i = 0; i < towers.Length; i++)
            {
                currentCoinsCountInGame += towers[i].CoinQuantity;
            }
        }
        _timeSinceLastSpawn = Time.time;

        playing = true;
    }

    public void StopGame()
    {
        playing = false;

        Coins[] coins = GameObject.FindObjectsOfType<Coins>();
        for (int i = 0; i < coins.Length; i++)
        {
            Destroy(coins[i].gameObject);
            currentCoinsCountInGame--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing)
            return;

        float ponderation = Mathf.Lerp(0.2f, 1, Mathf.InverseLerp(0, _maxCoinsInGame, currentCoinsCountInGame));
        if ((_timeSinceLastSpawn+_nextSpawn * ponderation - Time.time) <= 0 && currentCoinsCountInGame < _maxCoinsInGame)
        {
            SpawnAtRandomPos();
        }
    }

    private void SpawnAtRandomPos()
    {
        // Je récupère seulement les Spawner libre
        List<CoinsSpawner> availableSpawner = CoinsSpanwers.Where(spawner => spawner.IsFree).ToList();
        if (availableSpawner.Count < 1)
            return;

        int randomIndex = Random.Range(0, availableSpawner.Count - 1);
        availableSpawner[randomIndex].SpawnACoin(_coin);

        _timeSinceLastSpawn = Time.time;
        _nextSpawn = PickNextSpawn();

        currentCoinsCountInGame++;
    }

    private float PickNextSpawn()
    {
        return Random.Range(_minNextSpawn, _maxNextSpawn);
    }

    public void CoinConsumed()
    {
        currentCoinsCountInGame--;
        if (currentCoinsCountInGame < 0)
            currentCoinsCountInGame = 0;
    }
}
