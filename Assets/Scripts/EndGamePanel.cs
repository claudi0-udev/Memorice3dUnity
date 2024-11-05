using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Text gameTimeText;
    [SerializeField] private Text attempsText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;

    [SerializeField] private GameObject pauseButton, continueButton;

    public void SetGameTime(string gameTime)
    {
        gameTimeText.text = gameTime;
    }

    public void SetAttemps(int attemps)
    {
        attempsText.text = attemps.ToString();        
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
        GameSettings.lastGameScore_static = score;
        PlayerPrefs.SetInt("lastGameScore", score);
    }

    public void SetBestScore(int bestScore)
    {
        bestScoreText.text = bestScore.ToString();        
        GameSettings.bestGameScore_static = bestScore;
        PlayerPrefs.SetInt("bestGameScore", bestScore);
    }

    void OnEnable()
    {
        Debug.Log("EnGamePanel ENABLE_!");
        continueButton.SetActive(false);
        pauseButton.SetActive(false);
    }
}
