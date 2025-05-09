using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer;

public class ItemEvent
{
    public event Action<string> OnItemAdded;
    public void ItemAdded(string item)
    {
        OnItemAdded?.Invoke(item);
    }

    public event Action OnItemRemoved;

    public void ItemRemoved()
    {
        OnItemRemoved?.Invoke();
    }
}
