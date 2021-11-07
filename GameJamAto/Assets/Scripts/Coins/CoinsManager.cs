using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{

    private static CoinsManager instance;
    public static CoinsManager Instance { get => instance; }
    [SerializeField]
    private GameObject _coin;

    private int _nextSpawn;
    [SerializeField]
    private int _minNextSpawn;
    [SerializeField]
    private int _maxNextSpawn;

    private float _timeSinceLastSpawn = 0;

    [SerializeField]
    private int _maxCoinsOnGround;

    public int NumberOfCoinsActuallySpawn
    {
        get => CoinsSpanwers.Where(spawner => !spawner.IsFree).Count();
    }

    [SerializeField]
    public List<CoinsSpawner> CoinsSpanwers;

    // Start is called before the first frame update
    void Start()
    {
        instance = new CoinsManager();
        _nextSpawn = _minNextSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextSpawn < _timeSinceLastSpawn && NumberOfCoinsActuallySpawn < _maxCoinsOnGround)
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
        _nextSpawn = Random.Range(_minNextSpawn, _maxNextSpawn);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
