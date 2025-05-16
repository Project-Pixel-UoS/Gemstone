using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DayNightTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject openLaptopImage;
    public GameObject closedLaptopImage;
    public GameObject quizPanel;

    public AudioClip laptopCloseClip;
    public AudioClip cafeNightMusic;

    public UnityEvent OnNightTransition;

    // Handles all visual changes for night transition
    public void SwitchToNightVisuals()
    {
        openLaptopImage.SetActive(false);
        closedLaptopImage.SetActive(true);
        quizPanel.SetActive(false);
    }

    // Handles stopping current music and playing SFX + new music
    public void PlayNightAudio()
    {
        AudioManagement.instance.StopMusic();
        AudioManagement.instance.PlaySFX(laptopCloseClip);
        AudioManagement.instance.PlayMusic(cafeNightMusic);
    }

    // Invokes the event that triggers night-related actions (audio, animations etc.)
    public void InvokeNightTransitionEvent()
    {
        OnNightTransition.Invoke();
    }
}