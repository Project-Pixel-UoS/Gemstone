using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        generateQuestion();
    }

    /// <summary>
    /// Called when the correct answer is provided by the player.  
    /// Removes the current question from the list and generates a new question.
    /// </summary>
    public void correctAnswerProvided()
    {
        QnA.RemoveAt(currentQuestionID);
        generateQuestion();
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

            if (QnA[currentQuestionID].CorrectAnswerIndex == i + 1)
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
        currentQuestionID = Random.Range(0, QnA.Count);

        QuestionText.text = QnA[currentQuestionID].Question;

        SetAnswers();

        QnA.RemoveAt(currentQuestionID);
    }

}
