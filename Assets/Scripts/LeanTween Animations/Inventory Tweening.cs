using UnityEngine;

public class InventoryTweening : MonoBehaviour
{

    public LeanTweenType easeType;
    public bool isInventoryOpen;

    [SerializeField] private RectTransform inventoryPanel;  // UI Panel (must be RectTransform)
    [SerializeField] private float moveTime = 0.5f;         // Animation duration
    private float offScreenX;                               // X position when hidden
    private float onScreenX;                                // X position when visible

    void Start()
    {
        // Get panel width dynamically
        float inventoryWidth = inventoryPanel.rect.width * inventoryPanel.lossyScale.x / 2;

        // Calculate positions
        onScreenX = Screen.width - inventoryWidth; // Aligns inventory to the right
        offScreenX = Screen.width + inventoryWidth / 2; // Moves just enough offscreen

        // Start with inventory hidden
        inventoryPanel.anchoredPosition = new Vector2(offScreenX, inventoryPanel.anchoredPosition.y);

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
        LeanTween.moveX(gameObject, onScreenX, 1f).setDelay(moveTime).setEase(easeType);
        isInventoryOpen = true;
    }

    public void OnClose()
    {
        LeanTween.moveX(gameObject, offScreenX, 1f).setDelay(moveTime).setEase(easeType);
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
