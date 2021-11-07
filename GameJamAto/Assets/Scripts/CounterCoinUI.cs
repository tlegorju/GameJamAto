using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterCoinUI : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Tower tower;

    void Start() {
        SetStartText();
    }

    private void SetStartText() {
        while(tower == null) text.text = tower.GetCoins().ToString();
    }

    public void SetText(string txt) {
        text.text = txt;
    }
}
