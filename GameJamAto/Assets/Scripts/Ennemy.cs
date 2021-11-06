using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField]
    private int _lifePoint;
    public int LifePoint
    {
        get => _lifePoint;

        set
        {
            _lifePoint = value;
            if (_lifePoint >= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    [SerializeField]
    private float _speed;
    public float Speed { get => _speed; set => _speed = value; }

    [SerializeField]
    private int _damage;
    public int Damage { get => _damage; set => _damage = value; }

    [SerializeField]
    private Path _path;

    /// <summary>
    /// Index of the last point of the list through wich the ennemy passed
    /// </summary>
    private int _nextCheckPoint = 1;

    private float _travelCompletion = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MoveBetweenTwoPoints(_path.Points[_nextCheckPoint - 1], _path.Points[_nextCheckPoint]);
    }

    private void MoveBetweenTwoPoints(Transform ptDepart, Transform ptArrive)
    {
        transform.LookAt(ptArrive);
        transform.position = Vector3.Lerp(ptDepart.position, ptArrive.position, _travelCompletion);
        _travelCompletion += Time.deltaTime * Speed / Vector3.Distance(ptDepart.position, ptArrive.position);

        if (_travelCompletion > 1 && _nextCheckPoint < (_path.Points.Count - 1))
        {
            _nextCheckPoint++;
            _travelCompletion = 0;
        }
    }

    public void TakeDamages(int damage)
    {
        LifePoint -= damage;
    }

    public void DamagePlayer()
    {
        GameManager.Instance.TakeDamages(Damage);
    }
}
