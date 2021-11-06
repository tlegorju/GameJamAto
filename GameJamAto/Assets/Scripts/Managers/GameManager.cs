using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] private int LIFE_AT_START = 10;

    private int lifeLeft = 10;
    public int LifeLeft { get { return lifeLeft; } }

    private int score = 0;
    public int Score { get { return score; } }

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

    public void Reset()
    {
        lifeLeft = LIFE_AT_START;
        score = 0;
    }

    public void TakeDamages(int damages)
    {
        lifeLeft = Mathf.Max(0, lifeLeft - damages);
        //UIMananager.Instance.UpdateLifeLeft(lifeLeft);
        if(CheckForGameOver())
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

        //Play defeat sound
        //Show game over UI
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        //UPDATE UI SCORE
    }
}
