using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class WireTileHandling : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform initialParent; //Holds parent of dragged item before dragging begins
    TileSlot initialParentScript;
    [HideInInspector] public Transform parentAfterDrag; //Holds parent of dragged item before dragging begins

    private void Start()
    {
        initialParent = transform.parent;
        initialParentScript = initialParent.GetComponent<TileSlot>(); // Get the isEditable variable from the parent
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Dragging is disabled for this slot.");
            return; // Stop the drag process
        }

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Dragging is disabled for this slot.");
            return; // Stop the drag process
        }

        //These three lines make sure dragging and dropping actually works
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Dragging is disabled for this slot.");
            return; // Stop the drag process
        }
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
