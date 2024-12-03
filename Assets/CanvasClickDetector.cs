using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickDetector : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        DialogueHandler.FinishCurrentParagraph();
    }
}
