using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DialogueHandler.Display(DialogueHandler.FetchDialogue()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}