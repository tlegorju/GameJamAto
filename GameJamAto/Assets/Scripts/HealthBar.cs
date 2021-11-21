using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private Transform cam;
    private float lifeTo;

    private const float LerpSpeed = 3f;

    private void Awake() {
        cam = Camera.main.GetComponent<Transform>();    
    }

    private void Update()
    {
        if (slider.value == lifeTo)
        {
            return;
        }
        else if (Mathf.Abs(slider.value - lifeTo) < 0.001f)
        {
            slider.value = lifeTo;
        }
        else
        {
            slider.value = Mathf.Lerp(slider.value, lifeTo, Time.deltaTime * LerpSpeed);
        }
    }
    
    public void SetMaxLife(int life) {
        slider.value = 1;
        lifeTo = 1;
    }

    public void SetLife(float life)
    {
        lifeTo = Mathf.Clamp01(life);
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
