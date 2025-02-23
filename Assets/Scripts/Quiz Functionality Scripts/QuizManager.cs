using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <remarks>
/// Maintained by: Michael Edems-Eze
/// </remarks>
/// /// <todo>
/// - Prevent index errors when generating a question by ensuring currentQuestionID is valid.
/// </todo>

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Data")]
    public List<QuestionAnswerData> QnA;

    [Header("UI Elements")]
    public GameObject[] options;
    public int currentQuestionID;

    public TMP_Text QuestionText;

    private void Start()
    {
        QuestionText.text = QnA[currentQuestionID].Question;

        SetAnswers();
    }

    /// <summary>
    /// Called when the correct answer is provided by the player.  
    /// Removes the current question from the list and generates a new question.
    /// </summary>
    public void correctAnswerProvided()
    {
        generateQuestion();
    }

    public void incorrectAnswerProvided()
    {
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
            Debug.Log(currentQuestionID);
            options[i].GetComponent<AnswerProcedure>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestionID].Answers[i];

            if (QnA[currentQuestionID].CorrectAnswerIndex == i)
            {
                options[i].GetComponent<AnswerProcedure>().isCorrect = true;
            }

            Debug.Log(currentQuestionID);
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

    private float receivedSliderValue;

    // Method to receive slider value
    public void ReceiveSliderValue(float value)
    {
        receivedSliderValue = value;
        Debug.Log("Received slider value: " + receivedSliderValue);
        // You can now use this value in your quiz logic
    }
}
