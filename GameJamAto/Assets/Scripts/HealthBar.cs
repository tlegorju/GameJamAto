using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private Transform cam;
    private int lifeTo;

    private void Awake() {
        cam = Camera.main.GetComponent<Transform>();    
    }

    private void Update() {
        if(slider.value != lifeTo) { 
            float delta = lifeTo - slider.value;
            delta *= Time.deltaTime;
    
            slider.value += delta;
        }
    }
    
    public void SetMaxLife(int life) {
        slider.maxValue = life;
        slider.value = life;
    }

    public void SetLife(int life) {
        lifeTo = life;
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
