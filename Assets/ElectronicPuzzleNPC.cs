using UnityEngine;

public class ElectronicPuzzleNPC : MonoBehaviour
{
    public void TalktoNPC()
    {
        DialogueInstance dialogueInstance = new DialogueInstance("ElectronicPuzzle");
        dialogueInstance.StartDialogue();
    }
}
