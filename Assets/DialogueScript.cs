using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class DialogueScript : MonoBehaviour
{
    public Canvas canvas;
    private TextMeshProUGUI textElement;
    public IEnumerator DisplayDialogue(string text)
    {
        GameObject textObj = new GameObject("DynamicDialogueText");
        textObj.transform.SetParent(canvas.transform);

        textElement = textObj.AddComponent<TextMeshProUGUI>();
        //textElement.text = text;
        textElement.fontSize = 30;

        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height/4);
        rectTransform.anchoredPosition = new Vector2(0, -Screen.height / 3);


        GameObject backgroundObj = new GameObject("Background");
        backgroundObj.transform.SetParent(textObj.transform);

        Image backgroundImage = backgroundObj.AddComponent<Image>();
        backgroundImage.color = Color.black;

        rectTransform = backgroundObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 4);
        rectTransform.anchoredPosition = new Vector2(0, -Screen.height / 3);

        foreach (char c in text)
        {
            textElement.text += c;
            yield return new WaitForSeconds(0.2f);
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DisplayDialogue("abcdefghijklm"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
