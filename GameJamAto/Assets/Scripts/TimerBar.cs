using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Slider slider;
    private Transform cam;
    private float timeTo;

    private const float LerpSpeed = 2f;

    private void Awake() {
        cam = Camera.main.GetComponent<Transform>();    
    }

    private void Update() {
        
        if(Mathf.Abs(slider.value-timeTo)<0.001f)
        {
            slider.value = timeTo;
            return;
        }
        else
        {
            slider.value = Mathf.MoveTowards(slider.value, timeTo, Time.deltaTime * LerpSpeed);
        }
    }

    public void SetMaxTime()
    {
        slider.maxValue = 1;
        slider.value = 1;
    }

    public void SetTime(float time) {
        timeTo = Mathf.Clamp01(time);
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
