using System;
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
        if (Instance != null)
        {
            Debug.LogError("Found more than one Item Events Manager in the scene.");
            Destroy(gameObject);
        }
        Instance = this;
        itemEvents = new ItemEvent();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Utils.IsMouseClicked())
        {
            if(Utils.CheckMousePosInsideStage("Inventory")) SelectItem();
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
        currentItem = (item != null && (currentItem != item.gameObject || currentItem == null)) ? item.gameObject : null;
        Debug.Log(currentItem);
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
        if (DialogueHandler.IsActive())
        {
            return;
        }
        
        var item = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (item != null && item.transform.tag.Equals("Item"))
        {
            Transform itemSlot = GetEmptyInvSlot();
            item.transform.SetParent(itemSlot, false);
            item.transform.localScale = itemSlot.localScale;
            item.gameObject.layer = itemSlot.gameObject.layer;
            itemEvents.ItemAdded(item.name);//broadcast event 
            Debug.Log("Item picked up: " + item.name);
            DialogueInstance DI = new DialogueInstance(item.name);
            DI.StartDialogue();
        }
         
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
            Debug.Log("Item used");
            GameObject item = GameObject.Find(itemName); 
            Destroy(item);
            currentItem = null;
            itemEvents.ItemRemoved();
            return true;
        }
        return false;
    }

    /// <summary>
    /// get an array of items in inventory.
    /// </summary>
    /// <returns>all items in respective inventory slot.</returns>
    public Transform[] GetItems()
    {
        Transform[] inventory = new Transform[5];
        var hotbar = GameObject.Find("Hotbar").transform;
        int x = 0;
        foreach (Transform slot in hotbar)
        {
            if (slot.transform.childCount != 0) inventory[x] = slot.transform.GetChild(0);
        }
        return inventory;
    }

}
