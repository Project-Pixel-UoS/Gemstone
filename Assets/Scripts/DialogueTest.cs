using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueInstance dialogueInstance = new DialogueInstance("CafeCounter");
        dialogueInstance.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
