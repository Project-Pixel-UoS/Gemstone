using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickDetector : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer clicked 2");

        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        Debug.Log("Pointer clicked 3");
        DialogueHandler.FinishCurrentParagraph();
    }
}
