using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    private bool _isFree = true;
    public bool IsFree { get => _isFree; set => _isFree = value; }

    GameObject coinSpawned;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(!_isFree && coinSpawned == null);
        if (!_isFree && coinSpawned == null)
        {
            _isFree = true;
        }
    }

    public void SpawnACoin(GameObject coin)
    {
        coinSpawned = Instantiate(coin, transform);
        _isFree = false;
    }
}
