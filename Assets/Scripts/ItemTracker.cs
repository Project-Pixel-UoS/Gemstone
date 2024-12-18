using UnityEngine;
using Util;

/// <summary>
/// Tracks the current item selected
/// </summary>
public class ItemTracker : MonoBehaviour
{
    public static ItemTracker Instance { get; private set; }
    private GameObject item;
    public GameObject currentItem
    {
        get => item;
        set => item = value;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (Utils.IsMouseClicked())
        {
            SelectItem();
            print(currentItem);
        }
    }

    public void SelectItem()
    {
        var item = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("UI")).collider;
        currentItem = (item != null) ? item.gameObject : null;
    }

}
