using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Highscore
{
    public string pseudo;
    public int score;

    public Highscore(string p, int s) { pseudo = p; score = s; }
}

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private Transform MainMenuTransform;
    [SerializeField] private TextMeshProUGUI[] HighscoresText;
    [SerializeField] private Button PlayButton;

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
    [SerializeField] private Button GameOverContinueButton;

    [SerializeField] private Button OpenCreditButton;
    [SerializeField] private Transform CreditParent;
    [SerializeField] private Button CreditBackToMenuButton;

    [SerializeField] private Transform NewHighscoreParent;

    public const int NUMBER_OF_HIGHSCORE = 10;
    public const string HIGHSCORE_LABEL_PREF = "Highscore";
    [SerializeField] private Highscore[] highscores = new Highscore[NUMBER_OF_HIGHSCORE];

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
        PlayButton.onClick.AddListener(() => GameManager.Instance.StartPlay());
        OpenCreditButton.onClick.AddListener(() => OpenCredit());
        CreditBackToMenuButton.onClick.AddListener(() => CloseCredit());
        GameOverContinueButton.onClick.AddListener(() => GameManager.Instance.ContinueGameOver());
    }

    private void OpenCredit()
    {
        CreditParent.gameObject.SetActive(true);
        MainMenuTransform.gameObject.SetActive(false);
    }

    private void CloseCredit()
    {
        CreditParent.gameObject.SetActive(false);
        MainMenuTransform.gameObject.SetActive(true);
    }

    public void OpenNewHighscore()
    {
        NewHighscoreParent.gameObject.SetActive(true);
        CleanHUD();
        MainMenuTransform.gameObject.SetActive(false);
    }

    public void OpenMainMenu()
    {
        CleanHUD();
        MainMenuTransform.gameObject.SetActive(true);
        NewHighscoreParent.gameObject.SetActive(false);
        LoadHighscores();
        InitHighscoresVisual();
    }

    public void InitHighscoresVisual()
    {
        for (int i = 0; i < NUMBER_OF_HIGHSCORE; i++)
        {
            if (highscores[i] != null)
            {
                HighscoresText[i].text = (i + 1).ToString() + " - " + highscores[i].pseudo + " : " + highscores[i].score.ToString();
            }
            else
            {
                HighscoresText[i].text = (i + 1).ToString() + " - ...";
            }
        }
    }

    private void LoadHighscores()
    {
        string scoreString = PlayerPrefs.GetString(HIGHSCORE_LABEL_PREF, "");
        if (scoreString == "")
            return;

        string[] scoreSplit = scoreString.Split('\n');
        for (int i = 0; i < scoreSplit.Length; i++)
        {
            string[] stringData = scoreSplit[i].Split(':');
            if(stringData==null || stringData.Length<2)
            {
                continue;
            }
            Highscore data = new Highscore(stringData[0], int.Parse(stringData[1]));

            highscores[i] = data;
        }
    }

    public void SaveHighscores()
    {
        string stringData = "";
        for (int i = 0; i < highscores.Length && highscores[i]!=null; i++)
        {
            stringData += highscores[i].pseudo + ':' + highscores[i].score + '\n';
        }
        PlayerPrefs.SetString(HIGHSCORE_LABEL_PREF, stringData);
    }

    public void AddHighscore(string pseudo, int score)
    {
        Highscore newHighscore = new Highscore(pseudo, score);
        for (int i = 0; i < highscores.Length; i++)
        {
            if (highscores[i] == null || newHighscore.score >= highscores[i].score)
            {
                Highscore tmpHighscore = highscores[i];
                highscores[i] = newHighscore;
                newHighscore = tmpHighscore;
            }
        }
    }

    public bool CanAddHighscore(int score)
    {
        for (int i = 0; i < highscores.Length; i++)
        {
            if (highscores[i]== null || score >= highscores[i].score)
            {
                return true;
            }
        }
        return false;
    }

    public void InitGameHUD()
    {
        MainMenuTransform.gameObject.SetActive(false);

        HUDTransform.gameObject.SetActive(true);

        for (int i = 0; i < GameManager.Instance.LifeLeft; i++)
        {
            GameObject tmpHeart = Instantiate(HeartPrefab, HeartParent);
            tmpHeart.GetComponent<Image>().sprite = ActiveHeart;
        }

        CoinCounter.text = GameManager.Instance.CoinLeft.ToString() + " Coins";
        WaveCounter.text = "Wave 0";
        ScoreCounter.text = "Score : 0";
    }

    public void CleanHUD()
    {
        for (int i = HeartParent.childCount - 1; i >= 0; i--)
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
            if (i >= HeartParent.childCount)
                break;
            HeartParent.GetChild(i).GetComponent<Image>().sprite = GameManager.Instance.LifeLeft > i ? ActiveHeart : InactiveHeart;
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
        ScoreCounter.text = "Score : " + score.ToString();
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
        CleanHUD();
        GameOverParent.gameObject.SetActive(true);
        GameOverScoreText.text = "Score : " + score.ToString();
    }
}
