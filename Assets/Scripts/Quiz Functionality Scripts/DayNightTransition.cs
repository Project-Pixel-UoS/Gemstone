using UnityEngine;
using System.Collections;

public class DayNightTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject openLaptopImage;
    public GameObject closedLaptopImage;
    public GameObject quizPanel;

    public AudioClip laptopCloseClip;
    public AudioClip cafeNightMusic;

    // Update is called once per frame
    void Update()
    {

    }

    public void TransitionToNight()
    {

        openLaptopImage.SetActive(false);
        closedLaptopImage.SetActive(true);
        quizPanel.SetActive(false);

        StartCoroutine(HandleAudioTransition());
    }

    private IEnumerator HandleAudioTransition()
    {
        AudioManagement.instance.StopMusic();
        AudioManagement.instance.PlaySFX(laptopCloseClip);

        // Optional: wait for the SFX to finish before playing music
        if (laptopCloseClip != null)
            yield return new WaitForSeconds(laptopCloseClip.length);

        AudioManagement.instance.PlayMusic(cafeNightMusic);
    }
}
