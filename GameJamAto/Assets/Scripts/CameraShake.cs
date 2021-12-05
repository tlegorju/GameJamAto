using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance { get => instance; }

    private float duration = .07f;
    private float magnitude = 1f;
    private float accumulationStrenght = 0;

    Coroutine shakingCoroutine;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        shakingCoroutine = StartCoroutine(Shake());
    }

    private void Update()
    {

    }
    public void Init(float duration, float magnitude)
    {
        this.duration = duration;
        this.magnitude = magnitude;
    }

    public void ShakeCamera(float strength)
    {
        accumulationStrenght += strength;
    }

    private IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;

        while(true)
        {
            float elapsed = 0.0f;

            float x = Random.Range(-1f, 1f) * magnitude * accumulationStrenght;
            float y = Random.Range(-1f, 1f) * magnitude * accumulationStrenght;

            while (elapsed < duration)
            {

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPos+transform.right*x+transform.up*y, elapsed/duration);

                elapsed += Time.deltaTime;

                yield return null;
            }

            accumulationStrenght = Mathf.Max(accumulationStrenght - duration, 0);

            //transform.localPosition = originalPos;

        }
    }
}
