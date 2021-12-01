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
    [Range(5, 15)][SerializeField] private float lifetime = 5;
    [SerializeField] private int damages = 1;
    [SerializeField] private bool lookAtEnnemy = false;

    void Start()
    {

    }

    void Update()
    {
        if(lifetime <= 0 || target == null) Destroy(gameObject);
        lifetime -= Time.deltaTime;

        if(target != null) transform.position = Vector3.MoveTowards(transform.position, target.position + targetCollider.center, speed * Time.deltaTime);
        if(lookAtEnnemy)
            transform.rotation = Quaternion.LookRotation(target.position + targetCollider.center - transform.position);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Ennemy>() != null)
        {
            other.GetComponent<Ennemy>().TakeDamages(damages);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform tf) {
        target = tf;
        targetCollider = target.GetComponent<BoxCollider>();
    }
}
