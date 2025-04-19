using UnityEngine;
using UnityEngine.EventSystems;
public class Draghandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 placementLocation;
    private float snappingThreshold = 2f;
    public static bool isLocked = false;

    public void Start()
    {
        placementLocation = GameObject.Find("DetectorPlacement").transform.position;
        placementLocation.z = 0;
        Debug.Log("Placement pos " + placementLocation);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Locked? " + isLocked);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isLocked)
        {
            Debug.Log("Dragging");
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
        else
        {
            return;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Vector3.Distance(transform.position,placementLocation) < snappingThreshold)
        {
            transform.position = placementLocation;
        }
    }

}