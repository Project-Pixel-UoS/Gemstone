using UnityEngine;
using UnityEngine.EventSystems;

public class WireTileHandling : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Dragging");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("We're still dragging!");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("We finally stopped dragging...");
    }
}
