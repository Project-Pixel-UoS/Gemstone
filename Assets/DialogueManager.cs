using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public static class DialogueHandler
{
    public static float textTypingDelay = 0.08f;

    private static bool finishOnNextIter = false;
    //public static Sprite dialogueBGImage;

    private static TextMeshProUGUI textElement;
    public static IEnumerator Display(string text)
    {
        GameObject textObj = new GameObject("DynamicDialogueText");

        GameObject canvasObj = new GameObject("DialogueCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        textObj.transform.SetParent(canvas.transform);

        textElement = textObj.AddComponent<TextMeshProUGUI>();
        textElement.fontSize = 30;

        RectTransform rectTransformTXT = textObj.GetComponent<RectTransform>();
        rectTransformTXT.sizeDelta = new Vector2(Screen.width, Screen.height / 4);
        rectTransformTXT.anchoredPosition = new Vector2(0, -Screen.height / 3);



        ////GameObject backgroundObj = new GameObject("Background");
        //backgroundObj.transform.SetParent(textObj.transform);

        //Image backgroundImage = backgroundObj.AddComponent<Image>();
        //backgroundImage.sprite = dialogueBGImage;
        //backgroundImage.type = Image.Type.Filled;

        //RectTransform rectTransformBG = backgroundObj.GetComponent<RectTransform>();
        //rectTransformBG.sizeDelta = rectTransformTXT.sizeDelta; // Match text size

        //rectTransformBG.anchoredPosition = Vector2.zero;

        //backgroundObj.transform.SetSiblingIndex(0);
        textObj.transform.SetSiblingIndex(1);

        foreach (char c in text)
        {
            if (finishOnNextIter)
            {
                textElement.text = text;
                break;
            }
            else
            {
                textElement.text += c;
                yield return new WaitForSeconds(textTypingDelay);
            }
        }
    }
    public static void FinishCurrentParagraph()
    {
        finishOnNextIter = true;
    }
}
