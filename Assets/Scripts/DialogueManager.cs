using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using System.Globalization;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Diagnostics;
using Utils = Util.Utils;

public static class DialogueHandler
{
    private static bool finishOnNextIter = false;
    private static bool hasFinished = false;

    private static GameObject canvasObj = null;
    
    private static GameObject textObj = null;
    private static TextMeshProUGUI textElement = null;
    private static RectTransform rectTransformTXT = null;
    private static GameObject backgroundObj = null;
    private static Image backgroundImage = null;
    private static RectTransform rectTransformBG = null;
    
    private static GameObject speakerNameObj;
    private static TextMeshProUGUI speakerNameElement;
    private static GameObject speakerBGObj;
    private static Image speakerBGImage;
    private static RectTransform rectTransformSpeakerName;
    
    private static int textPaddingPX_X { get {return Utils.GetPercentScreenSizeX(3.5f);} }
    private static int textPaddingPX_Y { get {return Utils.GetPercentScreenSizeY(2f);} }
    
    private static Dictionary<string, int> dialogueHistory = new Dictionary<string, int>();
    public static IEnumerator Display(string text, float delay, bool skippable, string fontName, string speakerName = null)
    {
        if (Utils.DISABLE_ALL_DIALOGUE) { yield break; }
        Debug.Log("Display Dialogue called");
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
            textElement.fontSizeMin = Utils.GetPercentScreenSizeY(3.7f);
            textElement.fontSizeMax = Utils.GetPercentScreenSizeY(7.4f);
            textElement.textWrappingMode = TextWrappingModes.Normal;
            textElement.overflowMode = TextOverflowModes.Masking;
            textElement.color = Color.black;
            textElement.alignment = TextAlignmentOptions.Center;
        }
        textElement.font = Resources.Load<TMP_FontAsset>(fontName);

        if (rectTransformTXT == null)
        {
            rectTransformTXT = textObj.GetComponent<RectTransform>();
            rectTransformTXT.sizeDelta =
                new Vector2(Screen.width - 2 * textPaddingPX_X, Screen.height / 4 - textPaddingPX_Y - Utils.GetPercentScreenSizeY(2.7f));
            rectTransformTXT.anchoredPosition = new Vector2(0, -Screen.height / 3 - textPaddingPX_Y + Utils.GetPercentScreenSizeY(1.38f));
        }

        if (backgroundObj == null)
        {
            backgroundObj = new GameObject("Background");
            backgroundObj.transform.SetParent(canvasObj.GetComponent<Canvas>().transform);
            backgroundObj.transform.SetSiblingIndex(0);
        }

        if (backgroundImage == null)
        {
            backgroundImage = backgroundObj.AddComponent<Image>();
            backgroundImage.sprite = Resources.Load<Sprite>("Images/Dialogue/DialogueBG1");
            backgroundImage.type = Image.Type.Filled;
        }


        if (rectTransformBG == null)
        {
            rectTransformBG = backgroundObj.GetComponent<RectTransform>();
            rectTransformBG.sizeDelta = new Vector2(Screen.width, Screen.height / 4 + Utils.GetPercentScreenSizeY(2.7f));
            rectTransformBG.anchoredPosition = new Vector2(0, -Screen.height / 3);
        }

        if (speakerName != null)
        {
            if (speakerBGObj == null)
            {
                speakerBGObj = new GameObject("SpeakerBackground");
                speakerBGObj.transform.SetParent(canvasObj.GetComponent<Canvas>().transform);
                speakerBGObj.transform.SetSiblingIndex(2);

                speakerBGImage = speakerBGObj.AddComponent<Image>();
                speakerBGImage.sprite = Resources.Load<Sprite>("Images/Dialogue/DialogueBG1");
                speakerBGImage.type = Image.Type.Filled;
            }

            if (speakerNameObj == null)
            {
                speakerNameObj = new GameObject("SpeakerName");
                speakerNameObj.transform.SetParent(canvasObj.GetComponent<Canvas>().transform);
                speakerNameObj.transform.SetSiblingIndex(3);

                speakerNameElement = speakerNameObj.AddComponent<TextMeshProUGUI>();
                speakerNameElement.enableAutoSizing = true;
                speakerNameElement.fontSize = Utils.GetPercentScreenSizeY(6.5f);
                speakerNameElement.color = Color.black;
                speakerNameElement.alignment = TextAlignmentOptions.Center;
            }

            int flippedMultiplier = speakerName == "Player" || speakerName.IsUnityNull() ? 1 : -1;

            rectTransformSpeakerName = speakerNameObj.GetComponent<RectTransform>();
            rectTransformSpeakerName.sizeDelta = new Vector2(Utils.GetPercentScreenSizeX(15.6f), Utils.GetPercentScreenSizeY(7.4f));
            rectTransformSpeakerName.anchoredPosition = new Vector2(-1 * Utils.GetPercentScreenSizeX(30) * flippedMultiplier, rectTransformBG.anchoredPosition.y + rectTransformBG.sizeDelta.y / 2 + Utils.GetPercentScreenSizeY(0.9f));

            RectTransform rectTransformSpeakerBG = speakerBGObj.GetComponent<RectTransform>();
            rectTransformSpeakerBG.sizeDelta = new Vector2(Utils.GetPercentScreenSizeX(15.6f) * 1.3f, Utils.GetPercentScreenSizeY(7.4f) * 1.3f);
            rectTransformSpeakerBG.anchoredPosition = new Vector2(rectTransformSpeakerName.anchoredPosition.x, rectTransformSpeakerName.anchoredPosition.y);

            speakerNameElement.font = Resources.Load<TMP_FontAsset>(fontName);

            speakerNameElement.text = speakerName;
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
        }
        textElement.ForceMeshUpdate();

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
        // Debug.Log(!(canvasObj == null));
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

    private static void AddPlayedDialogue(string tag)
    {
        if (dialogueHistory.ContainsKey(tag))
        {
            dialogueHistory[tag]++;
        }
        else
        {
            dialogueHistory.Add(tag, 1);
        }
    }

    private static bool DialogueIsPlayed(string tag)
    {
        return dialogueHistory.ContainsKey(tag) && dialogueHistory[tag] > 0;
    }
    
    public static void PlayDialogue(string tag, bool ignoreLimit = false)
    {
        if (!ignoreLimit && DialogueIsPlayed(tag))
        {
            Debug.Log($"Dialogue {tag} has already been played {dialogueHistory[tag]} times.");
            return;
        }
        DialogueInstance dialogueInstance = new DialogueInstance(tag);
        dialogueInstance.StartDialogue();
        AddPlayedDialogue(tag);
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
        
        UnityEngine.Object.Destroy(speakerNameObj);
        UnityEngine.Object.Destroy(speakerNameElement);
        UnityEngine.Object.Destroy(speakerBGObj);
        UnityEngine.Object.Destroy(speakerBGImage);
        UnityEngine.Object.Destroy(rectTransformSpeakerName);
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
        string fontName = "normal";
        string speakerName = null;
        
        foreach (string line in dialogueText.Split('\n'))
        {
            var s = line.Trim().Split("|");

            if (s.Length > 1)
            {   
                typingDelay = s[1].Trim() == "-" ?  typingDelay : float.Parse(s[1]);
                skippable = s[2].Trim() == "-" ? skippable : Convert.ToBoolean(int.Parse(s[2]));
                fontName = s[3].Trim() == "-" ? fontName : s[3].Trim();
                speakerName = s[4].Trim() == "-" ? speakerName : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s[4].Trim());
            }
            dialogueLines.Add(new DialogueBlock(s[0], typingDelay, skippable, fontName, speakerName));
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
    public string fontName;
    public string speakerName;
    public DialogueBlock(string text, float letterDelay, bool skippable, string fontName, string speakerName) // Item dependancy waiting to be added
    {
        this.text = text;
        this.letterDelay = letterDelay;
        this.skippable = skippable;
        this.fontName = fontName;
        this.speakerName = speakerName;
    }
    public void Play()
    {
        // Actual call for dialogue being shown
        CoroutineRunner.Instance.RunCoroutine(DialogueHandler.Display(this.text, this.letterDelay, this.skippable, this.fontName, this.speakerName));
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