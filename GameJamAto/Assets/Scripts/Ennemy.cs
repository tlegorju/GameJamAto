using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    [SerializeField]
    private int MAX_LIFE_POINT;

    private int _lifePoint;
    public int LifePoint
    {
        get => _lifePoint;

        set
        {
            _lifePoint = value;
            healthBar.SetLife(((float)_lifePoint)/MAX_LIFE_POINT);
            if (_lifePoint <= 0 && !_isDead)
            {
                GameManager.Instance.AddScore(Score);
                Dies();
            }
            else
            {
                SoundManager.Instance.TryPlayNerdTakesDamageClip(audioSource);
            }
        }
    }

    public bool _isDead = false;

    [SerializeField]
    private float _speed;
    public float Speed { get => _speed; set => _speed = value; }

    [SerializeField]
    private int _damage;
    public int Damage { get => _damage; set => _damage = value; }

    [SerializeField]
    private int _score;
    public int Score { get => _score; set => _score = value; }

    [SerializeField]
    public Path _path;

    /// <summary>
    /// Index of the last point of the list through wich the ennemy passed
    /// </summary>
    private int _nextCheckPoint = 1;

    private float _travelCompletion = 0;

    private AudioSource audioSource;

    [SerializeField]
    private GameObject _deathParticule;
    [SerializeField]
    private Transform _deathParticuleTransform;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _lifePoint = MAX_LIFE_POINT;
        healthBar.SetMaxLife(LifePoint);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBetweenTwoPoints(_path.Points[_nextCheckPoint - 1], _path.Points[_nextCheckPoint]);
    }

    private void MoveBetweenTwoPoints(Transform ptDepart, Transform ptArrive)
    {
        if (_isDead)
            return;
        transform.LookAt(ptArrive);
        transform.position = Vector3.Lerp(ptDepart.position, ptArrive.position, _travelCompletion);
        _travelCompletion += Time.deltaTime * Speed / Vector3.Distance(ptDepart.position, ptArrive.position);

        if (_travelCompletion > 1 && _nextCheckPoint < (_path.Points.Count - 1))
        {
            _nextCheckPoint++;
            _travelCompletion = 0;
        }
        else if (_travelCompletion > 1)
        {
            DamagePlayer();
        }
    }

    public void TakeDamages(int damage)
    {
        if (!_isDead)
            LifePoint -= damage;
    }

    public void DamagePlayer()
    {
        if (_isDead)
            return;
        GameManager.Instance.TakeDamages(Damage);
        Dies();
    }

    public void Dies()
    {
        if (_isDead)
            return;
        _isDead = true;
        WaveManager.Instance.EnemyDies();
        SoundManager.Instance.TryPlayNerdDiesClip(audioSource);
        Instantiate(_deathParticule, _deathParticuleTransform);
        Invoke("DestroyObject", .3f);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
