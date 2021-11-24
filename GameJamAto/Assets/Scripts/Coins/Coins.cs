using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    int _lifeTimeMax = 10;

    float _lifeTime = 0;
    int _scaleDuration = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up * speed * Time.deltaTime);

        if (_lifeTime > _lifeTimeMax)
        {
            //Destroy(gameObject);
        }

        _lifeTime += Time.deltaTime;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerMovement>()?.PlayPickUpCoin();
            GameManager.Instance.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
