using UnityEngine;
using UnityEngine.EventSystems;
public class DialogueCanvasController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer clicked 1");
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        DialogueHandler.FinishCurrentParagraph();
    }
}
