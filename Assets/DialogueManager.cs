using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public static class DialogueHandler
{
    private static bool finishOnNextIter = false;
    private static TextMeshProUGUI textElement;
    private static bool hasFinished = false;
    public static IEnumerator Display(string text, float delay, bool skippable)
    {
        hasFinished = false;
        
        GameObject canvasObj = new GameObject("DialogueCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textObj = new GameObject("DynamicDialogueText");
        textObj.transform.SetParent(canvas.transform);
        textElement = textObj.AddComponent<TextMeshProUGUI>();
        textElement.fontSize = 30;
        textObj.transform.SetSiblingIndex(1);

        const int textPaddingPX_X = 30;
        const int textPaddingPX_Y = 20;
        RectTransform rectTransformTXT = textObj.GetComponent<RectTransform>();
        rectTransformTXT.sizeDelta = new Vector2(Screen.width - textPaddingPX_X, Screen.height / 4 - textPaddingPX_Y);
        rectTransformTXT.anchoredPosition = new Vector2(0 + textPaddingPX_X, -Screen.height / 3 - textPaddingPX_Y);

        // Background Image
        GameObject backgroundObj = new GameObject("Background");
        backgroundObj.transform.SetParent(canvas.transform);
        backgroundObj.transform.SetSiblingIndex(0);

        Image backgroundImage = backgroundObj.AddComponent<Image>();
        backgroundImage.sprite = Resources.Load<Sprite>("Images/Dialogue/DialogueBG2");
        backgroundImage.type = Image.Type.Filled;

        RectTransform rectTransformBG = backgroundObj.GetComponent<RectTransform>();
        rectTransformBG.sizeDelta = new Vector2(Screen.width, Screen.height / 4);
        rectTransformBG.anchoredPosition = new Vector2(0, -Screen.height / 3);
        
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
        
        finishOnNextIter = false;
        hasFinished = true;
    }
    public static void FinishCurrentParagraph()
    {
        if (!hasFinished)
        {
            finishOnNextIter = true;
        }
    }
    public static string FetchDialogueFromTag(string tag)
    {
        StreamReader sr = new StreamReader("Assets/Resources/Dialogue.txt");
        string dialogue = sr.ReadToEnd().Split($"[{tag}/]")[1].Split($"/{tag}")[0];
        sr.Close();
        return dialogue;
    }

    public static bool IsFinished()
    {
        return hasFinished;
    }

    public static void ClearDialogue()
    {
        if (!hasFinished)
        {
            textElement.text = "";
        }
    }
}

public class DialogueInstance
{
    public List<DialogueBlock> dialogueLines;
    private const float PAUSE_BETWEEN_BLOCKS = 4;
    // public DialogueInstance()
    // {
    //     dialogueLines = new List<DialogueBlock>();
    // }
    public DialogueInstance(string tag)
    {
        dialogueLines = new List<DialogueBlock>();
        LoadDialogueLines(tag);
    }
    private void LoadDialogueLines(string tag)
    {
        string dialogueText = DialogueHandler.FetchDialogueFromTag(tag);
        
        float typingDelay = 0.04f;
        bool skippable = true;
        
        foreach (string line in dialogueText.Split('\n'))
        {
            var s = line.Split("|");

            if (s.Length > 1)
            {
                dialogueLines.Add(new DialogueBlock(s[0], float.Parse(s[1]), Convert.ToBoolean(int.Parse(s[2]))));
                
                typingDelay = float.Parse(s[1]);
                skippable = Convert.ToBoolean(int.Parse(s[2]));
            }
            else
            {
                dialogueLines.Add(new DialogueBlock(s[0], typingDelay, skippable));
            }
        }
    }
    public void StartDialogue()
    {
        CoroutineRunner.Instance.RunCoroutine(this.PlayDialogueSequentially());
    }
    private IEnumerator PlayDialogueSequentially()
    {
        foreach (DialogueBlock line in dialogueLines)
        {
            var index = dialogueLines.IndexOf(line);
            if (index > 0)
            {
                DialogueHandler.ClearDialogue();
            }
            
            line.Play();
            
            while (!DialogueHandler.IsFinished())
            {
                yield return null;
            }

            yield return new WaitForSeconds(PAUSE_BETWEEN_BLOCKS);
        }
        
        
        // Destroy all the dialogue related things
    }
    public void FinishCurrentParagraph()
    {
        DialogueHandler.FinishCurrentParagraph();
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
        CoroutineRunner.Instance.RunCoroutine(DialogueHandler.Display(this.text, this.letterDelay, this.skippable));
    }
}

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("CoroutineRunner");
                _instance = obj.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
    
}