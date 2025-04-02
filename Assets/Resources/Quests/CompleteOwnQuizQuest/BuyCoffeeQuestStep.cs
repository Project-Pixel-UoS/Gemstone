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

    private void ItemAdded(string item)
    {
        if(item == "Coffee")
        {
            FinishQuestStep();
        }
    }

}
