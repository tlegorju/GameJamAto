using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    /* ENTRIES */
    [Header("Objets")]
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletSpawn;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private TimerBar timerBar;

    [Header("Paramètres de la tour")]
    [Range(3, 50)][SerializeField] private float viewRadius = 10f;
    [Range(0, 10)][SerializeField] private float firerate = .1f;
    [Range(1, 50)][SerializeField] private int maxTargetAtOnce = 2;
    [Range(1, 500)][SerializeField] private int coinQuantity = 3;
    [Range(1, 100)][SerializeField] private int coinDuration = 15;

    /* PROPERTIES */
    private Collider[] _targetsInViewRadius = {};
    private List<Collider> _targetsToShoot = new List<Collider>();
    private Vector3 currentTargetPos;
    [SerializeField]public int timer = 0;

    private AudioSource audioSource;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine("routineFindTargets", .2f);
        StartCoroutine("routineFireAtTargets", firerate/10);
    }

    void Update()
    {
        LookAtCurrentTarget();
    }

    void LookAtCurrentTarget() {
        if(currentTargetPos != null) {
            Vector3 targetDir = currentTargetPos - transform.position;
            targetDir.y = 0;
            Quaternion quat = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, quat, 500 * Time.deltaTime);
        }
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
        // Check if timer is empty then check if the tower has coins to add time to the timer
        if(timer <= 0 && coinQuantity > 0) {
            timer += coinDuration + 1;
            coinQuantity--;
        }
        if(timer > 0) {
            if(timer > coinDuration) {
                timer--;
                timerBar.SetMaxTime(timer);
                timerBar.SetTime(timer);
                StartCoroutine("CountDown");
            }
             _targetsToShoot.Clear();
            foreach(Collider collider in _targetsInViewRadius) {
                if(_targetsToShoot.Count <= (maxTargetAtOnce - 1) && collider != null) {
                    _targetsToShoot.Add(collider);
                }
            }
            foreach(Collider collider in _targetsToShoot) {
                if(collider != null) FireTarget(collider.transform);
            }
        }
       
    }

    IEnumerator CountDown() {
        while(timer > 0) {
            yield return new WaitForSeconds(1f);
            timer--;
            timerBar.SetTime(timer);
        }
        if(coinQuantity==0)
        {
            SoundManager.Instance.PlayTowerDeactivated(audioSource);
        }
    }

    void FireTarget(Transform tf) {
        currentTargetPos = tf.position;
        GameObject bullet = Instantiate(Bullet, BulletSpawn.position, Quaternion.LookRotation(tf.position - BulletSpawn.transform.position));
        bullet.GetComponent<Bullet>().SetTarget(tf);

        SoundManager.Instance.TryPlayTowerShootClip(audioSource);
    }

    public void SetCoins(int number) {
        coinQuantity = number;
    }

    public int GetCoins(int number) {
        return coinQuantity;
    }

    public void AddCoins(int number)
    {
        if (coinQuantity <= 0.01f)
            SoundManager.Instance.PlayTowerActivated(audioSource);
        coinQuantity += number;
    }

}
