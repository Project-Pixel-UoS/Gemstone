using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Maintained by: Michael Edems-Eze
/// </remarks>

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Data")]
    public List<QuestionAnswerData> QnA;

    [Header("UI Elements")]
    public GameObject[] options;
    public int currentQuestionID;

    public TMP_Text QuestionText;

    public GameObject mainPanel; // Main quiz UI (questions)
    public GameObject responsePanel; // Temporary response screen
    public TextMeshProUGUI responseText; // Text for feedback message

    [Header("Quiz Sections")]
    public List<GameObject> quizSections; // Assign sections in the Inspector
    public int currentSectionIndex = 0; //What Section we're on

    [Header("Spot the Difference")]
    public Toggle correctToggle; // Set this in the Inspector
    public ToggleGroup toggleGroup;
    public Button toggleSubmitButton;

    [Header("Slider Section")]
    public Slider answerSlider;
    public Button sliderSubmitButton;
    public float correctValue = 44f; // Set the correct slider value in the Inspector

    /// <summary>
    /// Shows the First Section of Laptop Quiz, and makes sure the toggle and slider buttons work for later sections
    /// </summary>
    private void Start()
    {
        ShowSection(0);

        QuestionText.text = QnA[currentQuestionID].Question;

        SetAnswers();

        toggleSubmitButton.onClick.AddListener(CheckSpotTheDifference);
        sliderSubmitButton.onClick.AddListener(CheckSliderAnswer);
    }

    #region Methods For Multiple Choice Questions

    /// <summary>
    /// Called when the correct answer is provided by the player.  
    /// Removes the current question from the list and generates a new question.
    /// </summary>
    public void correctAnswerProvided()
    {
        ShowResponse("Good Job! For now... ", 2);

        if (!((currentQuestionID + 1) < QnA.Count))
        {
            StartCoroutine(ShowResponseAndProceed("Correct!", 2));
        }
        generateQuestion();
    }

    /// <summary>
    /// Called when the incorrect answer is provided by the player.  
    /// Removes the current question from the list and generates a new question.
    /// </summary>
    public void incorrectAnswerProvided()
    {
        ShowResponse("Well... that's bad... ", 2);
        //Add bad message here
    }

    /// <summary>
    /// Sets the answer options for the current question.  
    /// Updates the text and assigns whether each option is correct or not.
    /// </summary>
    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerProcedure>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestionID].Answers[i];

            if (QnA[currentQuestionID].CorrectAnswerIndex == i)
            {
                options[i].GetComponent<AnswerProcedure>().isCorrect = true;
            }

        }
    }

    /// <summary>
    /// Randomly generates a new question from the list and displays it.  
    /// Removes the selected question from the list after it is displayed.
    /// </summary>
    void generateQuestion()
    {
        currentQuestionID++;

        if (currentQuestionID < QnA.Count)
        {           
            QuestionText.text = QnA[currentQuestionID].Question;

            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
        }    
    }

    #endregion

    #region Methods for Slider Section

    private float receivedSliderValue;

    /// <summary>
    /// Receives the slider value from the UI and updates the internal variable.
    /// </summary>
    /// <param name="value">The float value from the slider input.</param>
    public void ReceiveSliderValue(float value)
    {
        receivedSliderValue = value;
        // The received value can now be used in quiz logic.
    }


    public void CheckSliderAnswer()
    {
        if (Mathf.Approximately(answerSlider.value, correctValue)) // Allow minor float precision errors
        {
            StartCoroutine(ShowResponseAndProceed("Correct!", 2));
            return;
        }
        ShowResponse("Incorrect! Try again.", 2);
    }

    #endregion



    #region General Quiz Functions

    /// <summary>
    /// Displays a response message to the user for a specified duration.
    /// </summary>
    /// <param name="message">The message to be displayed.</param>
    /// <param name="duration">Duration for which the message will be shown (default: 2 seconds).</param>
    public void ShowResponse(string message, float duration = 2f)
    {
        StartCoroutine(DisplayResponse(message, duration));
    }

    /// <summary>
    /// Handles the display of the response message, temporarily hiding the main panel.
    /// </summary>
    /// <param name="message">The response message to be displayed.</param>
    /// <param name="duration">Time in seconds before the response disappears.</param>
    /// <returns>IEnumerator to handle the coroutine timing.</returns>
    private IEnumerator DisplayResponse(string message, float duration)
    {
        responseText.text = message;

        // Get the currently active section
        mainPanel = quizSections[currentSectionIndex];

        mainPanel.SetActive(false);

        responsePanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        responsePanel.SetActive(false);

        mainPanel.SetActive(true);
        
    }


    /// <summary>
    /// Activates the specified quiz section and deactivates all others.
    /// </summary>
    /// <param name="index">Index of the section to activate.</param>
    public void NextSection()
    {
        // Hide current section
        if (currentSectionIndex < quizSections.Count)
            quizSections[currentSectionIndex].SetActive(false);

        // Move to the next section
        currentSectionIndex++;

        // Show the next section if it exists
        if (currentSectionIndex < quizSections.Count)
            ShowSection(currentSectionIndex);
        else
            Debug.Log("Quiz Complete!");
    }

    /// <summary>
    /// Moves to the next quiz section if available.
    /// </summary>
    private void ShowSection(int index)
    {
        for (int i = 0; i < quizSections.Count; i++)
        {
            quizSections[i].SetActive(i == index);
        }
    }

    /// <summary>
    /// Shows a Message and moves to the Next Section
    /// </summary>
    private IEnumerator ShowResponseAndProceed(string message, float duration)
    {
        responseText.text = message;

        // Get the currently active section
        mainPanel = quizSections[currentSectionIndex];

        mainPanel.SetActive(false);

        responsePanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        responsePanel.SetActive(false);

        mainPanel.SetActive(true);

        NextSection();
    }

    #endregion

    /// <summary>
    /// Checks if the selected toggle is correct.
    /// </summary>
    public void CheckSpotTheDifference()
    {
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            if (toggle == correctToggle)
            {
                StartCoroutine(ShowResponseAndProceed("Well Done! Moving On...", 2));
                return;
            }
        }

        ShowResponse("You had a 50/50 shot and still failed...", 2);
    }

}
