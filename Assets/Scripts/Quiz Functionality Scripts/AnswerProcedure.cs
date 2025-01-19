using UnityEngine;

/// <summary>
/// Handles the logic for answering quiz questions and interacting with the QuizManager.
/// </summary>
/// /// <remarks>
/// Maintained by: Michael Edems-Eze
/// </remarks>
public class AnswerProcedure : MonoBehaviour
{
    /// <summary>
    /// Indicates whether this answer option is correct.
    /// </summary>
    public bool isCorrect = false;

    /// <summary>
    /// Reference to the QuizManager for managing quiz flow.
    /// </summary>
    public QuizManager quizManager;

    /// <summary>
    /// Called when the user selects an answer option.  
    /// If the answer is correct, triggers the correct answer logic in QuizManager; otherwise, triggers the incorrect logic.
    /// </summary>
    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct");
            quizManager.correctAnswerProvided();
        }
        else
        {
            Debug.Log("Not Correct");
            quizManager.correctAnswerProvided();
        }
    }
}
