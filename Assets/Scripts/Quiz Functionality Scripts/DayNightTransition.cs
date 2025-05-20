using UnityEngine;
using UnityEngine.UI;

public class DayNightTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject openLaptopImage;
    public GameObject closedLaptopImage;
    public GameObject zoomedInLaptopImage;
    public GameObject quizPanel;
    public Sprite nightCafe;
    public Sprite nightMainHall;

    private void Start()
    {
        nightCafe = Resources.Load<Sprite>("Sprites/NightCafe");
        nightMainHall = Resources.Load<Sprite>("Sprites/NightMainHall");
    }
        
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
        GameManager.instance.ChangeBackground(nightCafe, "Room: Cafe");
        GameManager.instance.ChangeBackground(nightMainHall, "Room: Main Hall");
        DoQuizQuestStep questStep = GameObject.Find("DoQuizQuestStep(Clone)").GetComponent<DoQuizQuestStep>();
        questStep.EndQuiz();
        GameManager.instance.isDay = false;
    }
}
