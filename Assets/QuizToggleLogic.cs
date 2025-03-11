using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizToggleLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text textfield;
    [SerializeField] private string message;


    public void SendMessage(bool toggleValue)
    {
        if (toggleValue)
        {
            textfield.SetText(message);
        } else
        {
            textfield.SetText(sourceText: "Nothing Set");
        }
    }

}
