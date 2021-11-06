using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public const int LIFE_AT_START = 10;

    private int lifeLeft = 10;
    public int LifeLeft { get { return lifeLeft; } }

    private int score = 0;
    public int Score { get { return score; } }

    private int waveNumber = 0;
    public int WaveNumber { get { return waveNumber; } }

    //[SerializeField] private PlayerController player;


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
        UIManager.Instance.OpenMainMenu();
    }

    public void Reset()
    {
        lifeLeft = LIFE_AT_START;
        score = 0;
        waveNumber = 0;
        UIManager.Instance.CleanHUD();
    }

    public void StartPlay()
    {
        UIManager.Instance.InitGameHUD();
        SoundManager.Instance.PlayMusic();
    }

    public void StopPlay()
    {

    }

    public void TakeDamages(int damages)
    {
        lifeLeft = Mathf.Max(0, lifeLeft - damages);
        UIManager.Instance.UpdateHearts(lifeLeft);
        if (CheckForGameOver())
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
        Debug.Log("Game Over");

        //Disable player control
        //Disable ennemies movement

        SoundManager.Instance.PlayGameOver();
        UIManager.Instance.GameOver(score);
        Invoke("GoBackToMainMenu", 3);
    }

    private void GoBackToMainMenu()
    {
        if(UIManager.Instance.CanAddHighscore(score))
        {
            UIManager.Instance.AddHighscore("THI", score);
        }
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
        SoundManager.Instance.PlayWaveStart();
    }

    public void WaveFinished()
    {
        SoundManager.Instance.PlayWaveFinish();
    }
}
