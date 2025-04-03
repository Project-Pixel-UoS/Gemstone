using UnityEngine;
using UnityEngine.EventSystems;
public class Draghandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 targetPos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        targetPos = new Vector3(-63, 340, 0);
        Debug.Log("Starting dragging");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Stopped dragging!");
    }

}