using UnityEngine;

public class AnswerProcedure : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;

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
