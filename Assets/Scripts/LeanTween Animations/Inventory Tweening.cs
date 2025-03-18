using UnityEngine;

public class InventoryTweening : MonoBehaviour
{
    public LeanTweenType easeType;
    public bool isInventoryOpen;

    [SerializeField] private RectTransform inventoryPanel;  // UI Panel (must be RectTransform)
    [SerializeField] private float transitionTime = 0.5f;         // Animation duration
    private float offScreenX;                               // X position when hidden
    private float onScreenX;                                // X position when visible

    void Start()
    {
        // Get panel width dynamically
        float inventoryWidth = inventoryPanel.rect.width * inventoryPanel.lossyScale.x;

        onScreenX = inventoryPanel.position.x;  // Store current position as on-screen
        offScreenX = onScreenX + inventoryWidth;  // Move 20 units to the right
    }

    public void ToggleInventory()
    {
        if (isInventoryOpen)
        {
            OnClose();
        } else
        {
            OnOpen();
        }
    }

    public void OnOpen()
    {
        LeanTween.moveX(gameObject, onScreenX, transitionTime).setEase(easeType);
        isInventoryOpen = true;
    }

    public void OnClose()
    {
        LeanTween.moveX(gameObject, offScreenX, transitionTime).setEase(easeType);
        isInventoryOpen = false;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
