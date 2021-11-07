using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    GameOver,
    NewHighscore
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public const int LIFE_AT_START = 10;

    private int lifeLeft = 10;
    public int LifeLeft { get { return lifeLeft; } }

    public const int COINS_AT_START = 5;

    private int coinLeft = 0;
    public int CoinLeft { get { return coinLeft; } }

    private int score = 0;
    public int Score { get { return score; } }

    private int waveNumber = 0;
    public int WaveNumber { get { return waveNumber; } }

    //[SerializeField] private PlayerController player;

    private GameState gameState;
    public GameState CurrentGameState { get { return gameState; } }

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        gameState = GameState.MainMenu;

        lifeLeft = LIFE_AT_START;
        score = 0;
        waveNumber = 0;
        coinLeft = COINS_AT_START;
        UIManager.Instance.CleanHUD();
        UIManager.Instance.OpenMainMenu();
    }

    public void StartPlay()
    {
        UIManager.Instance.InitGameHUD();
        SoundManager.Instance.PlayMusic();

        gameState = GameState.Playing;

        Invoke("StartNextWave", 3);
    }

    public void StopPlay()
    {

    }

    public void TakeDamages(int damages)
    {
        lifeLeft = Mathf.Max(0, lifeLeft - damages);
        UIManager.Instance.UpdateHearts(lifeLeft);
        if (gameState == GameState.Playing && CheckForGameOver())
        {
            TriggerGameOver();
        }
    }

    public bool CheckForGameOver()
    {
        return lifeLeft <= 0;
    }

    public void TriggerGameOver()
    {
        gameState = GameState.GameOver;

        //Disable player control
        //Disable ennemies movement

        SoundManager.Instance.PlayGameOver();
        UIManager.Instance.GameOver(score);

    }

    public void ContinueGameOver()
    {
        if (UIManager.Instance.CanAddHighscore(score))
        {
            gameState = GameState.NewHighscore;
            UIManager.Instance.OpenNewHighscore();
            return;
        }
        gameState = GameState.MainMenu;
        UIManager.Instance.OpenMainMenu();
    }

    public void GoBackToMainMenu()
    {
        UIManager.Instance.OpenMainMenu();
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        UIManager.Instance.UpdateScore(score);
    }

    public void StartNextWave()
    {
        waveNumber++;
        UIManager.Instance.UpdateWaves(waveNumber);
        UIManager.Instance.NewWave(waveNumber, 3);
        SoundManager.Instance.PlayWaveStart();
        WaveManager.Instance.StartWave(waveNumber);
    }

    public void WaveFinished()
    {
        SoundManager.Instance.PlayWaveFinish();
        if(gameState==GameState.Playing)
            Invoke("StartNextWave", 3);
    }

    public void AddCoin(int addedCoin)
    {
        coinLeft += addedCoin;
        UIManager.Instance.UpdateCoins(coinLeft);
    }

    public bool UseCoin()
    {
        if (coinLeft < 0)
            return false;

        coinLeft--;
        return true;
    }
}
