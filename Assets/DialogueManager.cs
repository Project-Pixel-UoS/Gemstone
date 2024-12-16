using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;

public static class DialogueHandler
{
    private static bool finishOnNextIter = false;
    private static TextMeshProUGUI textElement;
    private static bool hasFinished = false;
    public static IEnumerator Display(string text, float delay, bool skippable)
    {
        GameObject canvasObj = new GameObject("DialogueCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textObj = new GameObject("DynamicDialogueText");
        textObj.transform.SetParent(canvas.transform);
        textElement = textObj.AddComponent<TextMeshProUGUI>();
        textElement.fontSize = 30;

        const int textPaddingPX_X = 30;
        const int textPaddingPX_Y = 20;
        RectTransform rectTransformTXT = textObj.GetComponent<RectTransform>();
        rectTransformTXT.sizeDelta = new Vector2(Screen.width - textPaddingPX_X, Screen.height / 4 - textPaddingPX_Y);
        rectTransformTXT.anchoredPosition = new Vector2(0 + textPaddingPX_X, -Screen.height / 3 - textPaddingPX_Y);

        // Background Image
        GameObject backgroundObj = new GameObject("Background");
        backgroundObj.transform.SetParent(canvas.transform);

        Image backgroundImage = backgroundObj.AddComponent<Image>();
        backgroundImage.sprite = Resources.Load<Sprite>("Images/Dialogue/DialogueBG2");
        backgroundImage.type = Image.Type.Filled;

        RectTransform rectTransformBG = backgroundObj.GetComponent<RectTransform>();
        rectTransformBG.sizeDelta = new Vector2(Screen.width, Screen.height / 4);
        rectTransformBG.anchoredPosition = new Vector2(0, -Screen.height / 3);

        backgroundObj.transform.SetSiblingIndex(0);
        textObj.transform.SetSiblingIndex(1);

        // Text printing
        foreach (char c in text)
        {
            if (finishOnNextIter && !skippable)
            {
                textElement.text = text;
                break;
            }
            else
            {
                textElement.text += c;
                yield return new WaitForSeconds(delay);
            }
        }

        hasFinished = true;
    }
    public static void FinishCurrentParagraph()
    {
        finishOnNextIter = true;
    }
    public static string FetchDialogueFromTag(string tag)
    {
        StreamReader sr = new StreamReader("Assets/Resources/Dialogue.txt");
        string dialogue = sr.ReadToEnd().Split($"[{tag}/]")[1].Split($"/{tag}")[0];
        sr.Close();
        return dialogue;
    }
}

public class DialogueInstance
{
    public List<DialogueBlock> dialogueLines;
    public DialogueInstance()
    {
        dialogueLines = new List<DialogueBlock>();
    }
    public DialogueInstance(string tag)
    {
        dialogueLines = new List<DialogueBlock>();
        LoadDialogueLines(tag);
    }
    public void LoadDialogueLines(string tag)
    {
        string dialogueText = DialogueHandler.FetchDialogueFromTag(tag);
        
        foreach (string line in dialogueText.Split('\n'))
        {
            var s = line.Split("|");
            dialogueLines.Add(new DialogueBlock(s[0], float.Parse(s[1]), Convert.ToBoolean(int.Parse(s[2]))));
        }
    }
}
public class DialogueBlock
{
    public string text;
    public float letterDelay;
    public bool skippable;

    public DialogueBlock(string text, float letterDelay, bool skippable) // Item dependancy waiting to be added
    {
        this.text = text;
        this.letterDelay = letterDelay;
        this.skippable = skippable;
    }

    public void Play()
    {
        DialogueHandler.Display(this.text, this.letterDelay, this.skippable);
    }
    
    public void FinishCurrentParagraph()
    {
        DialogueHandler.FinishCurrentParagraph();
    }
}