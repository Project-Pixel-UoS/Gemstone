using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public void correctAnswerProvided()
    {
        QnA.RemoveAt(currentQuestionID);
        generateQuestion();
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerProcedure>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshPro>().text = QnA[currentQuestionID].Answers[i];

            if (QnA[currentQuestionID].CorrectAnswerIndex == i+1)
            {
                options[i].GetComponent<AnswerProcedure>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        currentQuestionID = Random.Range(0, QnA.Count);

        QuestionText.text = QnA[currentQuestionID].Question;

        SetAnswers();

        QnA.RemoveAt(currentQuestionID);

    }
}
