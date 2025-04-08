using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickDetector : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        DialogueHandler.FinishCurrentParagraph();
    }
}
