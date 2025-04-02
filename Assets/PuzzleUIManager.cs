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

    private void Start()
    {
        originalPosition = panel.anchoredPosition; // Store the original position
        canvasGroup.alpha = 0; // Start fully transparent
        panel.anchoredPosition = originalPosition - new Vector2(0, moveDistance); // Start slightly lower
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
}