using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Util;

/// <summary>
/// Tracks the current item selected
/// </summary>
public class ItemTracker : MonoBehaviour
{
    public static ItemTracker Instance { get; private set; }
    private GameObject item;
    public ItemEvent itemEvents;

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
            if (currentItem == null) PickupItem();
        }
    }

    /// <summary>
    /// selects/unselects item from the inv.
    /// </summary>
    public GameObject SelectItem()
    {
        var item = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("UI")).collider;
        //shorthand for: if *condition*, then currentItem = null, else = item.
        //can click twice on the same item to unselect.
        currentItem = (item == null || item.gameObject == currentItem) ? null : item.gameObject;
        return currentItem;
    }

    /// <summary>
    /// get the first empty item slot.
    /// </summary>
    /// <returns>gameobject of the empty inv slot.</returns>
    public Transform GetEmptyInvSlot()
    {
        var hotbar = GameObject.Find("Hotbar").transform;
        foreach(Transform child in hotbar)
        {
            if (child.transform.childCount == 0)
            {
                return child.transform;
            }
        }
        return null;
    }

    //Items in the room need the "Item" tag and be on the Default layer (or wtv
    //layer we decide to put the room in future).
    public void PickupItem()
    {
        var item = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (item != null && item.transform.tag.Equals("Item"))
        {
            Transform itemSlot = GetEmptyInvSlot();
            item.transform.SetParent(itemSlot, false);
            item.transform.localScale = itemSlot.localScale;
            item.gameObject.layer = itemSlot.gameObject.layer;
        }
        itemEvents.ItemAdded(); //broadcast event 
    }

    /// <summary>
    /// destroys the selected item if the right item is used.
    /// </summary>
    /// <param name="itemName">item needed to be used</param>
    /// <returns>true if correct item is used, false otherwise.</returns>
    public bool UseItem(String itemName)
    {
        if(itemName.Equals(currentItem.name))
        {
            GameObject item = GameObject.Find(itemName); 
            Destroy(item);
            currentItem = null;
            itemEvents.ItemRemoved();
            return true;
        }
        return false;
    }

    public Transform[] GetItems()
    {
        Transform[] inventory = new Transform[5];
        var hotbar = GameObject.Find("Hotbar").transform;
        int x = 0;
        foreach (Transform slot in hotbar)
        {
            inventory[x] = slot.transform.GetChild(0);
        }
        return inventory;
    }

}
