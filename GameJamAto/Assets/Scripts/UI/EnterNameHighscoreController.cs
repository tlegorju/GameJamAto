using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnterNameHighscoreController : MonoBehaviour
{
    [SerializeField] Button UpButton1;
    [SerializeField] TextMeshProUGUI TextLetter1;
    [SerializeField] Button DownButton1;
    private char letter1 = 'A';

    [SerializeField] Button UpButton2;
    [SerializeField] TextMeshProUGUI TextLetter2;
    [SerializeField] Button DownButton2;
    private char letter2 = 'A';

    [SerializeField] Button UpButton3;
    [SerializeField] TextMeshProUGUI TextLetter3;
    [SerializeField] Button DownButton3;
    private char letter3 = 'A';

    [SerializeField] Button continueButton;

    private void Awake()
    {
        UpButton1.onClick.AddListener(() => NextLetter1());
        DownButton1.onClick.AddListener(() => PreviousLetter1());
        UpButton2.onClick.AddListener(() => NextLetter2());
        DownButton2.onClick.AddListener(() => PreviousLetter2());
        UpButton3.onClick.AddListener(() => NextLetter3());
        DownButton3.onClick.AddListener(() => PreviousLetter3());

        continueButton.onClick.AddListener(() => SendName());
    }

    private void OnEnable()
    {
        letter1 = letter2 = letter3 = 'A';
        TextLetter1.text = TextLetter2.text = TextLetter3.text = "A";
    }

    public void NextLetter1()
    {
        letter1++;
        if (letter1 > 'Z')
            letter1 = 'A';
        TextLetter1.text = letter1.ToString();
    }

    public void PreviousLetter1()
    {
        letter1--;
        if (letter1 < 'A')
            letter1 = 'Z';
        TextLetter1.text = letter1.ToString();
    }
    public void NextLetter2()
    {
        letter2++;
        if (letter2 > 'Z')
            letter2 = 'A';
        TextLetter2.text = letter2.ToString();
    }

    public void PreviousLetter2()
    {
        letter2--;
        if (letter2 < 'A')
            letter2 = 'Z';
        TextLetter2.text = letter2.ToString();
    }
    public void NextLetter3()
    {
        letter3++;
        if (letter3 > 'Z')
            letter3 = 'A';
        TextLetter3.text = letter3.ToString();
    }

    public void PreviousLetter3()
    {
        letter3--;
        if (letter3 < 'A')
            letter3 = 'Z';
        TextLetter3.text = letter3.ToString();
    }


    public void SendName()
    {
        string name = letter1.ToString() + letter2.ToString() + letter3.ToString();
        UIManager.Instance.AddHighscore(name, GameManager.Instance.Score);
        UIManager.Instance.SaveHighscores();

        GameManager.Instance.Reset();
    }
}
