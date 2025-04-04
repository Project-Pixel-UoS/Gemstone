using UnityEngine;
using UnityEngine.UI;

public class PanelAnimator : MonoBehaviour
{
    public RectTransform panel; // The UI panel you want to animate
    public CanvasGroup canvasGroup; // For fade effect
    public float moveDistance = 50f; // How much it moves up
    public float duration = 0.5f; // Animation speed
    private bool isVisible = false; // Track panel visibility

    private Vector2 originalPosition;

    public GameObject[] puzzlePanels; // The UI panel you want to animate

    public RectTransform winPanel; // The UI panel you want to animate
    private CanvasGroup winGroup;     // For fading in the panel


    private void Start()
    {
        originalPosition = panel.anchoredPosition; // Store the original position
        canvasGroup.alpha = 0; // Start fully transparent
        panel.anchoredPosition = originalPosition - new Vector2(0, moveDistance); // Start slightly lower

        winGroup = winPanel.GetComponent<CanvasGroup>();
        if (winGroup == null)
        {
            // Add one if missing
            winGroup = winPanel.gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void TogglePanel()
    {
        if (isVisible)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        isVisible = true;

        // Move up & fade in
        LeanTween.moveY(panel, originalPosition.y, duration).setEaseOutExpo();
        LeanTween.alphaCanvas(canvasGroup, 1, duration).setEaseOutExpo();
    }

    public void HidePanel()
    {
        isVisible = false;

        // Move down & fade out
        LeanTween.moveY(panel, originalPosition.y - moveDistance, duration).setEaseInExpo();
        LeanTween.alphaCanvas(canvasGroup, 0, duration).setEaseInExpo();
    }

    public void DisplayWinMessage()
    {
        foreach (GameObject panel in puzzlePanels)
        {
            panel.SetActive(false);
        }

        // Enable the win panel
        winPanel.gameObject.SetActive(true);

        // Reset alpha
        winGroup.alpha = 0f;

        // Fade in the win panel
        LeanTween.alphaCanvas(winGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutCubic);

    }
}