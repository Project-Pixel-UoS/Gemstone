using UnityEngine;
using UnityEngine.UI;

public class ZoomInLaptop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject openLaptopImage;
    public GameObject closedLaptopImage;
    public GameObject zoomInLaptopImage;
    public GameObject enterQuizSection;
    public GameObject quizScreen;

    public Image fadeImage;

    // Fade to black
    public void FadeIn(float duration = 1f)
    {
        fadeImage.raycastTarget = true; // Block input if needed
        LeanTween.value(fadeImage.gameObject, SetAlpha, 0f, 1f, duration).setOnComplete(() => { ZoomIn(); });
    }

    // Fade from black
    public void FadeOut(float duration = 1f)
    {
        LeanTween.value(fadeImage.gameObject, SetAlpha, 1f, 0f, duration)
            .setOnComplete(() => { fadeImage.raycastTarget = false; }); // Allow input again
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }

    public void ZoomIn()
    {
        openLaptopImage.SetActive(false);
        closedLaptopImage.SetActive(false);
        zoomInLaptopImage.SetActive(true);
        enterQuizSection.SetActive(false);
        quizScreen.SetActive(true);
        FadeOut();
    }
}
