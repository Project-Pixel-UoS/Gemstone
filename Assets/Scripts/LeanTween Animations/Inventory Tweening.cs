using UnityEngine;

public class InventoryTweening : MonoBehaviour
{

    public LeanTweenType easeType;
    public float closeDelay;

    public void OnClose()
    {
        LeanTween.moveX(gameObject, 15f, 1f).setDelay(closeDelay).setEase(easeType);
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
