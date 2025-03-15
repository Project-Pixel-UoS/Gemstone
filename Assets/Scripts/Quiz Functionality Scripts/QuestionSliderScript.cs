using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSliderScript : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    public QuizManager quizManager; // Reference to the QuizManager script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            sliderText.text = v.ToString("0.00");
        }); 
    }
    public void SubmitSliderValue()
    {
        if (quizManager != null)
        {
            quizManager.ReceiveSliderValue(slider.value); // Send value to QuizManager
        }
        else
        {
            Debug.LogWarning("QuizManager reference not set in SliderHandler!");
        }
    }
}
