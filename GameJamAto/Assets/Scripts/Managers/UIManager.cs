using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private Transform MainMenuTransform;

    [SerializeField] private Transform HUDTransform;

    [SerializeField] private Transform HeartParent;
    [SerializeField] private GameObject HeartPrefab;
    [SerializeField] private Sprite ActiveHeart;
    [SerializeField] private Sprite InactiveHeart;

    [SerializeField] private Image CoinIcon;
    [SerializeField] private TextMeshProUGUI CoinCounter;

    [SerializeField] private TextMeshProUGUI WaveCounter;
    [SerializeField] private TextMeshProUGUI ScoreCounter;

    [SerializeField] private Transform NewWaveParent;
    [SerializeField] private TextMeshProUGUI NewWaveText;

    [SerializeField] private Transform GameOverParent;
    [SerializeField] private TextMeshProUGUI GameOverScoreText;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void InitGameHUD()
    {
        HUDTransform.gameObject.SetActive(true);

        for(int i=0;i<GameManager.Instance.LifeLeft; i++)
        {
            GameObject tmpHeart = Instantiate(HeartPrefab, HeartParent);
            tmpHeart.GetComponent<Image>().sprite = ActiveHeart;
        }

        CoinCounter.text = "0 Coins";
        WaveCounter.text = "Wave 0";
        ScoreCounter.text = "Score : 0";
    }

    public void CleanHUD()
    {
        for(int i=HeartParent.childCount-1;i>=0;i--)
        {
            Destroy(HeartParent.GetChild(i).gameObject);
        }
        NewWaveParent.gameObject.SetActive(false);
        GameOverParent.gameObject.SetActive(false);
        HUDTransform.gameObject.SetActive(false);
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

    public void NewWave(int waveNumber, float timeDisplay)
    {
        NewWaveParent.gameObject.SetActive(true);
        NewWaveText.text = "Wave " + waveNumber.ToString();

        StartCoroutine(DisableWaveTitle(timeDisplay));
    }

    private IEnumerator DisableWaveTitle(float timeDisplay)
    {
        yield return new WaitForSeconds(timeDisplay);
        NewWaveParent.gameObject.SetActive(false);
    }

    public void GameOver(int score)
    {
        GameOverParent.gameObject.SetActive(true);
        GameOverScoreText.text = "Score : " + score.ToString();
    }
}
