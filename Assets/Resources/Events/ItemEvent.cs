using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer;

public class ItemEvent
{
    public event Action OnItemAdded;
    public void ItemAdded()
    {
        OnItemAdded?.Invoke();
    }

    public event Action OnItemRemoved;

    public void ItemRemoved()
    {
        OnItemRemoved?.Invoke();
    }
}
