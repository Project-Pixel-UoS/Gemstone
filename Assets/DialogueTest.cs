using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DialogueHandler.Display("Some really awesome dialogue!? Some really awesome dialogue!? Some really awesome dialogue!? Some really awesome dialogue!? Some really awesome dialogue!? Some really awesome dialogue!? Some really awesome dialogue!? Some really awesome dialogue!? "));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
