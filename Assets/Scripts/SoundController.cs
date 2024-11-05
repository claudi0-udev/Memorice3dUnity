using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip clickSound;


    public void CorrectSoundPlay()
    {
        audioSource.clip = correctSound;
        audioSource.Play();
    }
    public void ErrorSoundPlay()
    {
        audioSource.clip = errorSound;
        audioSource.Play();
    }
    public void ClickSoundPlay()
    {
        audioSource.clip = clickSound;
        audioSource.Play();
    }

    


     
}
