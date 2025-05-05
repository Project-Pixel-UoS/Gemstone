using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a grid slot that can optionally hold and receive wire tiles.
/// </summary>
public class TileSlot : MonoBehaviour, IDropHandler
{
    public bool isEditable;

    public int x;
    public int y;

    /// <summary>
    /// Called when a draggable object is dropped on this slot.
    /// </summary>
    /// <param name="eventData">Event data related to the drop.</param>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void OnDrop(PointerEventData eventData)
    {
        if ((transform.childCount == 0) && isEditable)
        {
            GameObject dropped = eventData.pointerDrag;
            WireTileHandling draggableItem = dropped.GetComponent<WireTileHandling>();
            draggableItem.parentAfterDrag = transform;
        }
    }
}
