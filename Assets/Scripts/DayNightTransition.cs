using UnityEngine;

public class DayNightTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject openLaptopImage;
    public GameObject closedLaptopImage;
    public GameObject quizPanel;

    // Update is called once per frame
    void Update()
    {

    }

    public void TransitionToNight() {

        openLaptopImage.SetActive(false);
        closedLaptopImage.SetActive(true);
        quizPanel.SetActive(false);
    }
}
