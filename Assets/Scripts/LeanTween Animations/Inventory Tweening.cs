using UnityEngine;

public class InventoryTweening : MonoBehaviour
{

    public LeanTweenType easeType;
    public float closeDelay;
    public bool isInventoryOpen;

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
        LeanTween.moveX(gameObject, 0f, 1f).setDelay(closeDelay).setEase(easeType);
        isInventoryOpen = true;
    }

    public void OnClose()
    {
        LeanTween.moveX(gameObject, 15f, 1f).setDelay(closeDelay).setEase(easeType);
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
