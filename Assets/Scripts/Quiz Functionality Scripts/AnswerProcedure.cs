using UnityEngine;

/// <summary>
/// Handles the logic for answering quiz questions and interacting with the QuizManager.
/// </summary>
/// <remarks>
/// Maintained by: Michael Edems-Eze
/// </remarks>
/// <todo>
/// - Add transitions when all questions are completed (e.g., fade to nighttime).
/// </todo>
public class AnswerProcedure : MonoBehaviour
{
    // Indicates whether this answer option is correct.
    public bool isCorrect = false;

    // Reference to the QuizManager for managing quiz flow.
    public QuizManager quizManager;

    // Reference to the DayNightTransition for triggering day/night cycle changes.
    public DayNightTransition dayNightScript;

    // Counter to track the number of questions answered.
    int answeredQuestionsNum = 0;

    /// <summary>
    /// Called when the user selects an answer option.  
    /// If the answer is correct, triggers the correct answer logic in QuizManager; otherwise, triggers the incorrect logic.
    /// </summary>
    public void Answer()
    {
        // Check if the selected answer is correct and handle accordingly.
        if (isCorrect)
        {
            Debug.Log("Correct");

            // Increment the number of answered questions.
            answeredQuestionsNum++;

            Debug.Log(answeredQuestionsNum);

            quizManager.correctAnswerProvided();
            
        }
        else
        {
            Debug.Log("Not Correct");
            quizManager.incorrectAnswerProvided(); 
        }                
    }
}
