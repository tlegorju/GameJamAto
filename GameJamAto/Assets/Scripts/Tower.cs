using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    /* ENTRIES */
    [Header("Objets")]
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletSpawn;

    [Header("Paramètres")]
    [Range(3, 50)][SerializeField] private float viewRadius = 10f;
    [SerializeField] private LayerMask enemyMask;
    [Range(0, 10)][SerializeField] private float firerate = .1f;
    [Range(1, 50)][SerializeField] private int maxTargetAtOnce = 2;

    /* PROPERTIES */
    private Collider[] _targetsInViewRadius = {};
    private List<Collider> _targetsToShoot = new List<Collider>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
    
    void Start()
    {
        StartCoroutine("routineFindTargets", .2f);
        StartCoroutine("routineFireAtTargets", firerate/100);
    }

    void Update()
    {
        
    }

    IEnumerator routineFindTargets(float delay) {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    IEnumerator routineFireAtTargets(float delay) {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GetAndFireAtTargets();
        }
    }

    void FindTargets() {
        _targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
    }

    void GetAndFireAtTargets() {
        _targetsToShoot.Clear();
        foreach(Collider collider in _targetsInViewRadius) {
            if(_targetsToShoot.Count <= maxTargetAtOnce) {
                _targetsToShoot.Add(collider);
            }
        }
        foreach(Collider collider in _targetsToShoot) {
            FireTarget(collider.transform);
        }
    }

    void FireTarget(Transform tf) {
        Instantiate(Bullet, BulletSpawn.position, Quaternion.LookRotation(tf.position - BulletSpawn.transform.position));
    }
}
