using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickDetector : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Canvas clicked!");
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        DialogueHandler.FinishCurrentParagraph();
    }
}
