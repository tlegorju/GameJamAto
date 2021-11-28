 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /* ENTRIES */
    [Header("Objets")]
    private Transform target;
    private BoxCollider targetCollider;

    [Header("Paramètres de la boulette")]
    [Range(5, 500)][SerializeField] private float speed = 150f;
    [Range(5, 15)][SerializeField] private int lifetime = 5;
    [SerializeField] private int damages = 1;

    void Start()
    {
        StartCoroutine("LifetimeCountdown");
    }

    void Update()
    {
        if(lifetime <= 0 || target == null) Destroy(gameObject);
        if(target != null) transform.position = Vector3.MoveTowards(transform.position, target.position + targetCollider.center, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Collide");
        if(other.GetComponent<Ennemy>() != null)
        {
            Debug.Log("CollideEnemy " + other.gameObject.name);
            other.GetComponent<Ennemy>().TakeDamages(damages);
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
        targetCollider = target.GetComponent<BoxCollider>();
    }
}
