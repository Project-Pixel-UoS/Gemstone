using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI panel animations such as showing, hiding, and fading, 
/// as well as win message display logic for a puzzle game.
/// </summary>
public class PanelAnimator : MonoBehaviour
{
    /// <summary>
    /// The main UI panel to animate.
    /// </summary>
    public RectTransform panel;

    /// <summary>
    /// The CanvasGroup attached to the panel for controlling fade effects.
    /// </summary>
    public CanvasGroup canvasGroup;

    /// <summary>
    /// Distance the panel should move upward when showing.
    /// </summary>
    public float moveDistance = 50f;

    /// <summary>
    /// Duration of the show/hide animation in seconds.
    /// </summary>
    public float duration = 0.5f;

    /// <summary>
    /// Tracks whether the panel is currently visible.
    /// </summary>
    private bool isVisible = false;

    /// <summary>
    /// The original anchored position of the panel.
    /// </summary>
    private Vector2 originalPosition;

    /// <summary>
    /// Array of puzzle-related panels that are hidden when the win message is shown.
    /// </summary>
    public GameObject[] puzzlePanels;

    /// <summary>
    /// The win panel that is shown when the puzzle is completed.
    /// </summary>
    public RectTransform winPanel;

    /// <summary>
    /// CanvasGroup attached to the win panel for fade-in effects.
    /// </summary>
    private CanvasGroup winGroup;

    /// <summary>
    /// Initializes panel position and visibility states.
    /// </summary>
    private void Start()
    {
        originalPosition = panel.anchoredPosition;
        canvasGroup.alpha = 0;
        panel.anchoredPosition = originalPosition - new Vector2(0, moveDistance);

        winGroup = winPanel.GetComponent<CanvasGroup>();
        if (winGroup == null)
        {
            winGroup = winPanel.gameObject.AddComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// Toggles the panel visibility. Shows it if hidden, hides it if visible.
    /// </summary>
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

    /// <summary>
    /// Animates the panel to move upward and fade in.
    /// </summary>
    public void ShowPanel()
    {
        isVisible = true;

        LeanTween.moveY(panel, originalPosition.y, duration).setEaseOutExpo();
        LeanTween.alphaCanvas(canvasGroup, 1, duration).setEaseOutExpo();
    }

    /// <summary>
    /// Animates the panel to move downward and fade out.
    /// </summary>
    public void HidePanel()
    {
        isVisible = false;

        LeanTween.moveY(panel, originalPosition.y - moveDistance, duration).setEaseInExpo();
        LeanTween.alphaCanvas(canvasGroup, 0, duration).setEaseInExpo();
    }

    /// <summary>
    /// Hides all puzzle panels and displays the win message panel with a fade-in effect.
    /// </summary>
    public void DisplayWinMessage()
    {
        foreach (GameObject panel in puzzlePanels)
        {
            panel.SetActive(false);
        }

        winPanel.gameObject.SetActive(true);
        winGroup.alpha = 0f;
        LeanTween.alphaCanvas(winGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutCubic);
    }
}
