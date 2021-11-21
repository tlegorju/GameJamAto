using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    [SerializeField] private bool rotateX, rotateY, rotateZ;
    [SerializeField] private float minRotationSpeed, maxRotationSpeed;
    
    private float speedRotationX, speedRotationY, speedRotationZ;

    // Start is called before the first frame update
    void Start()
    {
        speedRotationX = rotateX ? Random.Range(minRotationSpeed, maxRotationSpeed) * (Random.Range(0,2) > 0 ? 1:-1) : 0;
        speedRotationY = rotateY ? Random.Range(minRotationSpeed, maxRotationSpeed) * (Random.Range(0, 2) > 0 ? 1 : -1) : 0;
        speedRotationZ = rotateZ ? Random.Range(minRotationSpeed, maxRotationSpeed) * (Random.Range(0, 2) > 0 ? 1 : -1) : 0;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.Euler(speedRotationX * Time.deltaTime, speedRotationY * Time.deltaTime, speedRotationZ * Time.deltaTime);

        transform.rotation *= rotation;
    }
}
