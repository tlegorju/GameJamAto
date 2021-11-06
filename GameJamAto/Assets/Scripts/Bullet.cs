 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /* ENTRIES */
    [Header("Objets")]
    private Transform target;

    [Header("Paramètres de la boulette")]
    [Range(5, 100)][SerializeField] private float speed = 150f;
    [Range(5, 15)][SerializeField] private int lifetime = 5;
    [Range(1, 10)][SerializeField] private int power = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LifetimeCountdown");
    }

    void Update()
    {
        Debug.Log(target);
        if(lifetime <= 0 || target == null) Destroy(gameObject);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Ennemy>() != null) {
            other.GetComponent<Ennemy>().TakeDamages(power);
            Destroy(gameObject);
        }
    }

    private IEnumerator LifetimeCountdown() {
        while(lifetime > 0) {
            yield return new WaitForSeconds(1f);
            lifetime--;
        }
    }

    public void SetTarget(Transform tf) {
        target = tf;
    }
}
