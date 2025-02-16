using UnityEngine;
using System;

public class ItemEvent
{
    public event Action OnItemAdded;
    public void ItemAdded()
    {
        if (OnItemAdded != null) OnItemAdded();
    }

    public event Action OnItemRemoved;

    public void ItemRemoved()
    {
        if (OnItemRemoved != null) OnItemRemoved();
    }
}
