using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCoffeeQuestStep : QuestStep
{
    private void OnEnable()
    {
        ItemTracker.Instance.itemEvents.OnItemAdded += ItemAdded;
    }

    private void OnDisable()
    {
        ItemTracker.Instance.itemEvents.OnItemAdded -= ItemAdded;
    }

    private void ItemAdded()
    {
        print(ItemTracker.Instance.GetItems());
        FinishQuestStep();
    }

}
