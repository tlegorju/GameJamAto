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
    private int currentCoinsCountInGame;


    [SerializeField]
    public List<CoinsSpawner> CoinsSpanwers;

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
        instance = new CoinsManager();
        _nextSpawn = PickNextSpawn();

        currentCoinsCountInGame = GameManager.Instance.CoinLeft;
        Tower[] towers = GameObject.FindObjectsOfType<Tower>();
        if(towers!=null)
        {
            for(int i=0;i<towers.Length;i++)
            {
                currentCoinsCountInGame += towers[i].CoinQuantity;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextSpawn < _timeSinceLastSpawn && currentCoinsCountInGame < _maxCoinsInGame)
        {
            SpawnAtRandomPos();
        }
        else
        {
            _timeSinceLastSpawn += Time.deltaTime;
        }
    }

    private void SpawnAtRandomPos()
    {
        // Je récupère seulement les Spawner libre
        List<CoinsSpawner> availableSpawner = CoinsSpanwers.Where(spawner => spawner.IsFree).ToList();
        int randomIndex = Random.Range(0, availableSpawner.Count - 1);
        availableSpawner[randomIndex].SpawnACoin(_coin);

        _timeSinceLastSpawn = 0;
        _nextSpawn = PickNextSpawn();
    }

    private float PickNextSpawn()
    {
        return Random.Range(_minNextSpawn, _maxNextSpawn);
    }

    public void CoinConsumed()
    {
        currentCoinsCountInGame--;
    }
}
