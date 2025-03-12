using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;

public static class DialogueHandler
{
    private static bool finishOnNextIter = false;
    private static TextMeshProUGUI textElement = null;
    private static TextMeshProUGUI textElementName = null;
    private static bool hasFinished = false;

    private static GameObject canvasObj = null;
    private static GameObject textObj = null;
    private static GameObject textObjName = null;
    private static RectTransform rectTransformTXT = null;
    private static GameObject backgroundObj = null;
    private static Image backgroundImage = null;
    private static RectTransform rectTransformBG = null;
    
    private const int textPaddingPX_X = 30;
    private const int textPaddingPX_Y = 20;

    public static IEnumerator Display(string text, float delay, bool skippable, string font_name)
    {
        Debug.Log("Display called");
        hasFinished = false;

        if (canvasObj == null)
        {
            canvasObj = new GameObject("DialogueCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if (textObj == null || textElement == null)
        {
            textObj = new GameObject("DynamicDialogueText");
            textObj.transform.SetParent(canvasObj.GetComponent<Canvas>().transform);
            textElement = textObj.AddComponent<TextMeshProUGUI>();
            textElement.enableAutoSizing = true;
            textElement.fontSizeMin = 20;
            textElement.fontSizeMax = 45;
            textElement.color = Color.black;
        }
        textElement.font = Resources.Load<TMP_FontAsset>($"{font_name}");
        if (textObjName == null || textElementName == null)
        {
            textObjName = new GameObject("DynamicDialogueText");
            textObjName.transform.SetSiblingIndex(1);
            textElementName = textObjName.AddComponent<TextMeshProUGUI>();
            Debug.Log(textElementName.IsUnityNull());
            textElementName.enableAutoSizing = true;
            textElementName.fontSizeMin = 20;
            textElementName.fontSizeMax = 45;
            textElementName.color = Color.black;
        }
        textElementName.font = textElement.font = Resources.Load<TMP_FontAsset>($"{font_name}");
        textElementName.rectTransform.sizeDelta = new Vector2(Screen.width - textPaddingPX_X, Screen.height / 4 - textPaddingPX_Y);
        textElementName.text = "its findwiubgdoiauwgdaoidgawdpiaugwdpiue";

        if (rectTransformTXT == null)
        {
            rectTransformTXT = textObj.GetComponent<RectTransform>();
            rectTransformTXT.sizeDelta =
                new Vector2(Screen.width - textPaddingPX_X, Screen.height / 4 - textPaddingPX_Y);
            rectTransformTXT.anchoredPosition = new Vector2(0 + textPaddingPX_X, -Screen.height / 3 - textPaddingPX_Y);
            textElement.rectTransform.sizeDelta =
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
        
        // Debug.Log(textElement.font.faceInfo.familyName);
        if (textElement.font.faceInfo.familyName == "Strange Path")
        {
            text = text.ToUpper();
        }
        
        // Text printing
        foreach (char c in text)
        {
            if (finishOnNextIter && skippable)
            {
                textElement.text = text;
                break;
            }
            else
            {
                textElement.text += c;
                yield return new WaitForSeconds(delay);
            }
            textElement.fontSize = textElement.fontSize * 0.7f;
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

    public static bool IsActive()
    {
        Debug.Log(!(canvasObj == null));
        return !(canvasObj == null);
    }
    public static string FetchDialogueFromTag(string tag)
    {
        StreamReader sr = new StreamReader("Assets/Resources/Dialogue.txt");
        string dialogue = sr.ReadToEnd().Split($"[{tag}/]")[1].Split($"[/{tag}]")[0].Trim();
        sr.Close();
        if (dialogue.Length > 1) { return dialogue; }
        Debug.Log("No dialogue found for this tag");
        return "No dialogue found for this tag";
    }

    public static bool IsFinished()
    {
        return hasFinished;
    }

    public static void ClearDialogue()
    {
        if (hasFinished && textElement != null)
        {
            textElement.text = "";
            textElement.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public static void DeleteTempObjs()
    {
        UnityEngine.Object.Destroy(canvasObj);
        UnityEngine.Object.Destroy(textElement);
        UnityEngine.Object.Destroy(textObj);
        UnityEngine.Object.Destroy(rectTransformTXT);
        UnityEngine.Object.Destroy(backgroundObj);
        UnityEngine.Object.Destroy(backgroundImage);
        UnityEngine.Object.Destroy(rectTransformBG);
        UnityEngine.Object.Destroy(textElementName);
    }

    public static void ToggleDarkOverlay()
    {
        if (GameObject.Find("DarkScreenOverlay").IsUnityNull())
        {
            GameObject overlay = new GameObject("DarkScreenOverlay");
            overlay.transform.SetParent(GameObject.Find("Canvas").transform, false);
            Image overlayImage = overlay.AddComponent<Image>();
            overlayImage.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
            
            RectTransform rectTransform = overlay.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            
            overlay.transform.SetSiblingIndex(overlay.transform.childCount - 2);
        }
        else
        {
            UnityEngine.Object.Destroy(GameObject.Find("DarkScreenOverlay"));
        }
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
        
        float typingDelay = 0.025f;
        bool skippable = true;
        string font_name = "normal";
        
        foreach (string line in dialogueText.Split('\n'))
        {
            var s = line.Trim().Split("|");

            if (s.Length > 1)
            {
                typingDelay = float.Parse(s[1]);
                skippable = Convert.ToBoolean(int.Parse(s[2]));
                font_name = s[3].Trim();
            }
            dialogueLines.Add(new DialogueBlock(s[0], typingDelay, skippable, font_name));
            
        }
    }
    public void StartDialogue()
    {
        // Debug.Log("Starting Dialogue");
        CoroutineRunner.Instance.RunCoroutine(this.PlayDialogueSequentially());
    }
    private IEnumerator PlayDialogueSequentially()
    {
        DialogueHandler.ToggleDarkOverlay();
        foreach (DialogueBlock line in dialogueLines)
        {
            DialogueHandler.ClearDialogue();
            
            line.Play();
            
            while (!DialogueHandler.IsFinished())
            {
                yield return null;
            }

            yield return new WaitForSeconds(PAUSE_BETWEEN_BLOCKS);
        }
        DialogueHandler.ToggleDarkOverlay();
        
        
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
    public string font_name;
    public DialogueBlock(string text, float letterDelay, bool skippable, string font_name) // Item dependancy waiting to be added
    {
        this.text = text;
        this.letterDelay = letterDelay;
        this.skippable = skippable;
        this.font_name = font_name;
    }
    public void Play()
    {
        // Actual call for dialogue being shown
        CoroutineRunner.Instance.RunCoroutine(DialogueHandler.Display(this.text, this.letterDelay, this.skippable, font_name));
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