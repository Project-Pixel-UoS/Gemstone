using UnityEngine;
using UnityEngine.EventSystems;

public class WireTileHandling : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform parentAfterDrag; //Holds parent of dragged item before dragging begins

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Dragging");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); //Places dragged object in front of everything else
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("We're still dragging!");

        //These three lines make sure dragging and dropping actually works
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("We finally stopped dragging...");
        transform.SetParent(parentAfterDrag);
    }
}
