using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles logic for updating a TMP text field based on the state of a toggle.
/// When enabled, it displays a predefined message; otherwise, it resets to "Nothing Set".
/// </summary>
public class QuizToggleLogic : MonoBehaviour
{
    /// <summary>
    /// Reference to the TMP_Text field that will be updated.
    /// </summary>
    [SerializeField] private TMP_Text textfield;

    /// <summary>
    /// The message to display when the toggle is enabled.
    /// </summary>
    [SerializeField] private string message;

    /// <summary>
    /// Updates the text field based on the toggle's value.
    /// </summary>
    /// <param name="toggleValue">Boolean value indicating whether the toggle is on (true) or off (false).</param>
    public void SendMessage(bool toggleValue)
    {
        if (toggleValue)
        {
            textfield.SetText(message);
        }
        else
        {
            textfield.SetText(sourceText: "Nothing Set");
        }
    }
}