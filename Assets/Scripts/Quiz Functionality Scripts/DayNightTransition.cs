using UnityEngine;

public class DayNightTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject openLaptopImage;
    public GameObject closedLaptopImage;
    public GameObject zoomedInLaptopImage;
    public GameObject quizPanel;

    // Update is called once per frame
    void Update()
    {

    }

    public void TransitionToNight()
    {
        openLaptopImage.SetActive(false);
        closedLaptopImage.SetActive(true);
        zoomedInLaptopImage.SetActive(false);
        quizPanel.SetActive(false);
        DoQuizQuestStep questStep = GameObject.Find("DoQuizQuestStep(Clone)").GetComponent<DoQuizQuestStep>();
        questStep.EndQuiz();
    }
}
