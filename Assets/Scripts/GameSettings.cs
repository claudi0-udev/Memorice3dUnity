using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static float backgroundMusicVolume_static;
    //public float backgroundMusicVolume;
    [SerializeField] private Slider backgroundMusicVolume_slider;
    public static int gameDifficulty_static;
    //public int gameDifficulty;
    [SerializeField] private Slider gameDifficulty_slider;

    [SerializeField] private AudioSource backgroundMusicAudioSource;

    public static int lastGameScore_static;
    public static int bestGameScore_static;
    //public int lastGameScore;
    



    void Awake()
    {
        GetBackgroundMusicVolume();
        GetGameDifficulty();
        GetGameScores();
        if(backgroundMusicAudioSource) DontDestroyOnLoad(backgroundMusicAudioSource);
        Time.timeScale = 1;
    }

    public void GetBackgroundMusicVolume()
    {
        backgroundMusicVolume_static = PlayerPrefs.GetFloat("backgroundMusicVolume", .5f);
        if(backgroundMusicVolume_slider != null) backgroundMusicVolume_slider.value = backgroundMusicVolume_static;

        
        if(backgroundMusicAudioSource) backgroundMusicAudioSource.volume = backgroundMusicVolume_static;
        Debug.Log("GetBackgroundMusicVolume");
    }

    public void SetBackgroundMusicVolume()
    {
        
        backgroundMusicVolume_static = backgroundMusicVolume_slider.value;

        PlayerPrefs.SetFloat("backgroundMusicVolume", backgroundMusicVolume_static);

        if(backgroundMusicAudioSource) backgroundMusicAudioSource.volume = backgroundMusicVolume_static;
        Debug.Log("SetBackgroundMusicVolume");
    }

    public void GetGameDifficulty()
    {
        gameDifficulty_slider.gameObject.SetActive(false);
        gameDifficulty_static = PlayerPrefs.GetInt("gameDifficulty", 4);

        if(gameDifficulty_slider != null) gameDifficulty_slider.value = (float)gameDifficulty_static;
        Debug.Log("GetGameDifficulty " + gameDifficulty_static);

        gameDifficulty_slider.gameObject.SetActive(true);
    }

    public void SetGameDifficulty()
    {
        gameDifficulty_static = ((int)gameDifficulty_slider.value);
        PlayerPrefs.SetInt("gameDifficulty", gameDifficulty_static);
        Debug.Log("SetGameDifficulty " + gameDifficulty_static );
    }

    public void GetGameScores()
    {
        bestGameScore_static = PlayerPrefs.GetInt("bestGameScore", 0);
        lastGameScore_static = PlayerPrefs.GetInt("lastGameScore", 0);
    }


}
