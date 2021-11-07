using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Slider slider;
    private Transform cam;
    private int timeTo;

    private void Awake() {
        cam = Camera.main.GetComponent<Transform>();    
    }

    private void Update() {
        if(slider.value != timeTo) { 
            float delta = timeTo - slider.value;
            delta *= Time.deltaTime;
    
            slider.value += delta;
        }
    }
    
    public void SetMaxTime(int time) {
        slider.maxValue = time;
        slider.value = time;
    }

    public void SetTime(int time) {
        timeTo = time;
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
