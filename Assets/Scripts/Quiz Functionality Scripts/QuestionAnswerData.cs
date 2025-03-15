using UnityEngine;

/// <summary>
/// Represents the data structure for a quiz question and its associated answers.
/// </summary>
/// <remarks>
/// Maintained by: Michael Edems-Eze
/// </remarks>

[System.Serializable]
public class QuestionAnswerData
{
    public string Question;
    public string[] Answers;
    public int CorrectAnswerIndex;
}