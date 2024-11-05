using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsActions : MonoBehaviour
{
    void Awake()
    {
        ToTransparentBlackPanel();
    }

    public Image backgroundImage; 
    public void ExitGame()
    {
        StartCoroutine(ExitGameIEnumerator());
    }

    IEnumerator ExitGameIEnumerator()
    {
        ToBlackScreen();
        yield return new WaitForSeconds(2.5f);
        Application.Quit();
        Debug.Log("Exit Game");
        yield break;
    }

    public void PlayGame(string sceneName)
    {
        StartCoroutine(PlayGameIEnumerator(sceneName));
    }

    IEnumerator PlayGameIEnumerator(string sceneName)
    {
        ToBlackScreen();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Game Start!");
        yield break;
    }

    public void ToBlackScreen()
    {
        backgroundImage.CrossFadeAlpha(1, 1.5f, false);
    }

    public void ToTransparentBlackPanel()
    {
        backgroundImage.CrossFadeAlpha(0, 1.5f, false);
    }
}
