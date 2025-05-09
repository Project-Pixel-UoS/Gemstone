using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI panel animations such as showing, hiding, and fading, 
/// as well as win message display logic for a puzzle game.
/// </summary>
/// <remarks>
/// Maintained by: Michael Edems-Eze
/// </remarks>
public class PanelAnimator : MonoBehaviour
{
    public RectTransform panel;           // The main UI panel to animate
    public CanvasGroup canvasGroup;       // CanvasGroup for fade in/out effect

    public float moveDistance = 50f;      // Distance panel moves upward when shown
    public float duration = 0.5f;         // Duration of the animation

    private bool isVisible = false;       // Tracks whether the panel is currently visible
    private Vector2 originalPosition;     // Original anchored position of the panel

    public GameObject[] puzzlePanels;     // All puzzle UI panels to be hidden on win

    public RectTransform winPanel;        // The win message panel
    private CanvasGroup winGroup;         // CanvasGroup for fading in the win panel

    /// <summary>
    /// Initializes panel position and visibility states.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    private void Start()
    {
        originalPosition = panel.anchoredPosition; // Store original panel position
        canvasGroup.alpha = 0; // Set panel to fully transparent
        panel.anchoredPosition = originalPosition - new Vector2(0, moveDistance); // Move it downward off-screen

        // Ensure the win panel has a CanvasGroup for fade animation
        winGroup = winPanel.GetComponent<CanvasGroup>();
        if (winGroup == null)
        {
            winGroup = winPanel.gameObject.AddComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// Toggles the panel visibility. Shows it if hidden, hides it if visible.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
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
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void ShowPanel()
    {
        isVisible = true;

        LeanTween.moveY(panel, originalPosition.y, duration).setEaseOutExpo();
        LeanTween.alphaCanvas(canvasGroup, 1, duration).setEaseOutExpo();
    }

    /// <summary>
    /// Animates the panel to move downward and fade out.
    /// </summary>
    /// /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void HidePanel()
    {
        isVisible = false;

        LeanTween.moveY(panel, originalPosition.y - moveDistance, duration).setEaseInExpo();
        LeanTween.alphaCanvas(canvasGroup, 0, duration).setEaseInExpo();
    }

    /// <summary>
    /// Hides all puzzle panels and displays the win message panel with a fade-in effect.
    /// </summary>
    /// /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void DisplayWinMessage()
    {
        // Deactivate all puzzle panels
        foreach (GameObject panel in puzzlePanels)
        {
            panel.SetActive(false);
        }

        // Show and fade in the win panel
        winPanel.gameObject.SetActive(true);
        winGroup.alpha = 0f;
        LeanTween.alphaCanvas(winGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutCubic);
    }
}
