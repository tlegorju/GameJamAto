using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private Transform HUDTransform;

    [SerializeField] private Transform HeartParent;
    [SerializeField] private GameObject HeartPrefab;
    [SerializeField] private Sprite ActiveHeart;
    [SerializeField] private Sprite InactiveHeart;

    [SerializeField] private Image CoinIcon;
    [SerializeField] private TextMeshProUGUI CoinCounter;

    [SerializeField] private TextMeshProUGUI WaveCounter;
    [SerializeField] private TextMeshProUGUI ScoreCounter;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void Init()
    {
        for(int i=0;i<GameManager.Instance.LifeLeft; i++)
        {
            GameObject tmpHeart = Instantiate(HeartPrefab, HeartParent);
            tmpHeart.GetComponent<Image>().sprite = ActiveHeart;
        }

        CoinCounter.text = "0 Coins";
        WaveCounter.text = "Wave 0";
        ScoreCounter.text = "Score : 0";
    }

    public void Reset()
    {
        for(int i=HeartParent.childCount-1;i>=0;i--)
        {
            Destroy(HeartParent.GetChild(i).gameObject);
        }
    }

    public void UpdateHearts(int lifeLeft)
    {
        for (int i = 0; i < GameManager.LIFE_AT_START; i++)
        {
            HeartParent.GetChild(i).GetComponent<Image>().sprite = GameManager.Instance.LifeLeft>i? ActiveHeart : InactiveHeart;
        }
    }

    public void UpdateCoins(int coinsCount)
    {
        CoinCounter.text = coinsCount.ToString() + " Coins";
    }

    public void UpdateWaves(int waveCount) 
    {
        WaveCounter.text = "Wave " + waveCount.ToString();
    }
    public void UpdateScore(int score)
    {
        ScoreCounter.text = "Wave " + score.ToString();
    }
}
