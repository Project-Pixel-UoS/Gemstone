using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCoffeeQuestStep : QuestStep
{
    private void Start()
    {
        ItemTracker.Instance.itemEvents.OnItemAdded += ItemAdded;
    }
    private void OnEnable()
    {
        if(ItemTracker.Instance != null) ItemTracker.Instance.itemEvents.OnItemAdded += ItemAdded;
    }

    private void OnDisable()
    {
        ItemTracker.Instance.itemEvents.OnItemAdded -= ItemAdded;
    }

    /// <summary>
    /// When event is broadcasted from ItemTracker, check if the item is coffee.
    /// Finish quest when coffee is added.
    /// </summary>
    /// <param name="item"></param>
    private void ItemAdded(string item)
    {
        if(item == "Coffee")
        {
            FinishQuestStep();
        }
    }

}
