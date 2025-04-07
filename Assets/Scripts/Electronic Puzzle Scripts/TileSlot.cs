using UnityEngine;
using UnityEngine.EventSystems;

public class TileSlot : MonoBehaviour, IDropHandler
{
    public bool isEditable; //If the slot is able to be edited

    public int x;                   // X coordinate in the grid
    public int y;                   // Y coordinate in the grid
    public void OnDrop(PointerEventData eventData)
    {        

        if ((transform.childCount == 0) && (isEditable))
        {
            GameObject dropped = eventData.pointerDrag;
            WireTileHandling draggableItem = dropped.GetComponent<WireTileHandling>();
            draggableItem.parentAfterDrag = transform;
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
