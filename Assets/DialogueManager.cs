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

    private static GameObject canvasObj = null;
    private static GameObject textObj = null;
    private static RectTransform rectTransformTXT = null;
    private static GameObject backgroundObj = null;
    private static Image backgroundImage = null;
    private static RectTransform rectTransformBG = null;
    
    private const int textPaddingPX_X = 30;
    private const int textPaddingPX_Y = 20;
    public static IEnumerator Display(string text, float delay, bool skippable)
    {
        hasFinished = false;

        if (canvasObj == null)
        {
            canvasObj = new GameObject("DialogueCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if (textObj == null)
        {
            textObj = new GameObject("DynamicDialogueText");
            textObj.transform.SetParent(canvasObj.GetComponent<Canvas>().transform);
            textElement = textObj.AddComponent<TextMeshProUGUI>();
            textElement.fontSize = 30;
            textObj.transform.SetSiblingIndex(1);
        }

        if (rectTransformTXT == null)
        {
            rectTransformTXT = textObj.GetComponent<RectTransform>();
            rectTransformTXT.sizeDelta =
                new Vector2(Screen.width - textPaddingPX_X, Screen.height / 4 - textPaddingPX_Y);
            rectTransformTXT.anchoredPosition = new Vector2(0 + textPaddingPX_X, -Screen.height / 3 - textPaddingPX_Y);
        }

        if (backgroundObj == null)
        {
            // Background Image
            backgroundObj = new GameObject("Background");
            backgroundObj.transform.SetParent(canvasObj.GetComponent<Canvas>().transform);
            backgroundObj.transform.SetSiblingIndex(0);
        }

        if (backgroundImage == null)
        {
            backgroundImage = backgroundObj.AddComponent<Image>();
            backgroundImage.sprite = Resources.Load<Sprite>("Images/Dialogue/DialogueBG2");
            backgroundImage.type = Image.Type.Filled;
        }


        if (rectTransformBG == null)
        {
            rectTransformBG = backgroundObj.GetComponent<RectTransform>();
            rectTransformBG.sizeDelta = new Vector2(Screen.width, Screen.height / 4);
            rectTransformBG.anchoredPosition = new Vector2(0, -Screen.height / 3);
        }

        // bool hasFinishedPara = false;
        
        // Text printing
        foreach (char c in text)
        {
            if (finishOnNextIter && skippable)
            {
                textElement.text = text;
                // hasFinishedPara = true;
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
        string dialogue = sr.ReadToEnd().Split($"[{tag}/]")[1].Split($"[/{tag}]")[0].Trim();
        sr.Close();
        return dialogue;
    }

    public static bool IsFinished()
    {
        return hasFinished;
        // hi
    }

    public static void ClearDialogue()
    {
        if (hasFinished)
        {
            textElement.text = "";
            textElement.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public static void DeleteTempObjs()
    {
        UnityEngine.Object.Destroy(canvasObj);
        UnityEngine.Object.Destroy(textObj);
        UnityEngine.Object.Destroy(rectTransformTXT);
        UnityEngine.Object.Destroy(backgroundObj);
        UnityEngine.Object.Destroy(backgroundImage);
        UnityEngine.Object.Destroy(rectTransformBG);
    }
}

public class DialogueInstance
{
    public List<DialogueBlock> dialogueLines;
    private const float PAUSE_BETWEEN_BLOCKS = 2.5f;
    
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
                typingDelay = float.Parse(s[1]);
                skippable = Convert.ToBoolean(int.Parse(s[2]));
            }
            dialogueLines.Add(new DialogueBlock(s[0], typingDelay, skippable));
            
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
            // var index = dialogueLines.IndexOf(line);
            
            // Debug.Log($"{index} - {line.text}");
            
            DialogueHandler.ClearDialogue();
            
            line.Play();
            
            while (!DialogueHandler.IsFinished())
            {
                yield return null;
            }

            yield return new WaitForSeconds(PAUSE_BETWEEN_BLOCKS);
        }
        
        
        // Destroy all the dialogue related things
        DialogueHandler.DeleteTempObjs();
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