using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DayNightTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject openLaptopImage;
    public GameObject closedLaptopImage;
    public GameObject zoomedInLaptopImage;
    public GameObject quizPanel;
    private Sprite nightCafe;
    private Sprite nightMainHall;
    public AudioClip laptopCloseClip;
    public AudioClip cafeNightMusic;

    private void Start()
    {
        nightCafe = Resources.Load<Sprite>("Sprites/NightCafe");
        nightMainHall = Resources.Load<Sprite>("Sprites/NightMainHall");
    }

    public UnityEvent OnNightTransition;

    // Handles all visual changes for night transition
    public void SwitchToNightVisuals()
    {
        openLaptopImage.SetActive(false);
        closedLaptopImage.SetActive(true);
        zoomedInLaptopImage.SetActive(false);
        quizPanel.SetActive(false);
        GameManager.instance.ChangeBackground(nightCafe, "Room: Cafe");
        GameManager.instance.ChangeBackground(nightMainHall, "Room: Main Hall");
        DoQuizQuestStep questStep = GameObject.Find("DoQuizQuestStep(Clone)").GetComponent<DoQuizQuestStep>();
        questStep.EndQuiz();
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