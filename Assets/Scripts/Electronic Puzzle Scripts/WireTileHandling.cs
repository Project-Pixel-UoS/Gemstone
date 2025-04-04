using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Direction
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3
}

[Serializable]
public class DirectionConnection
{
    public Direction direction;
    public bool isConnected;
}

public class WireTileHandling : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2Int gridPosition { get; private set; }

    public CircuitPuzzleManager puzzleManager;  // Reference to the CircuitPuzzleManager


    public Color onColor = Color.green;  // Color when the wire is "on"
    public Color offColor = Color.red;  // Color when the wire is "off"

    public bool isWireOn = false; // Track whether the wire is "on"

    public Image image;
    TileSlot initialParentScript;
    [HideInInspector] public Transform initialParent; //Holds parent of dragged item before dragging begins
    [HideInInspector] public Transform parentAfterDrag; //Holds parent of dragged item before dragging begins

    [SerializeField] public List<DirectionConnection> connectionsList = new List<DirectionConnection> //Makes a list of objects first so they can be set and show up in Inspector
    {
        new DirectionConnection { direction = Direction.Top, isConnected = false },
        new DirectionConnection { direction = Direction.Right, isConnected = false },
        new DirectionConnection { direction = Direction.Bottom, isConnected = false },
        new DirectionConnection { direction = Direction.Left, isConnected = false }
    };

    private int rotationState = 0;

    private void Start()
    {
        initialParent = transform.parent;
        initialParentScript = initialParent.GetComponent<TileSlot>(); // Get the isEditable variable from the parent

        // Set initial color (could be off by default)
        UpdateWireColor();

        puzzleManager = GetComponentInParent<CircuitPuzzleManager>();
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

    public void RotateTile()
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Rotation is NOT Allowed.");
            return; // Stop the drag process
        }

        rotationState = (rotationState + 1) % 4; // Cycle through 0, 90, 180, 270 degrees
        transform.Rotate(0, 0, -90);

        // Store the last connection before rotating
        bool lastConnection = connectionsList[3].isConnected;

        // Shift connections clockwise (left → bottom → right → top)
        for (int i = 3; i > 0; i--)
        {
            connectionsList[i].isConnected = connectionsList[i - 1].isConnected;
        }
        connectionsList[0].isConnected = lastConnection;

        // Call the LightUpConnectedWires function to update the circuit
        if (puzzleManager != null)
        {
            puzzleManager.LightUpConnectedWires();
        }
    }

    private void UpdateWireColor()
    {
        if (image != null)
        {
            image.color = isWireOn ? onColor : offColor;
        }
    }

    // Turn the wire ON
    public void TurnOn()
    {
        isWireOn = true;
        UpdateWireColor();
    }

    // Turn the wire OFF
    public void TurnOff()
    {
        isWireOn = false;
        UpdateWireColor();
    }

    public List<Direction> GetConnectedDirections()
    {
        List<Direction> connectedDirections = new List<Direction>();

        foreach (var conn in connectionsList)
        {
            if (conn.isConnected)
            {
                connectedDirections.Add(conn.direction);
            }
        }

        Debug.Log($"Wire at {gridPosition} has connections: {string.Join(", ", connectedDirections)}");

        return connectedDirections;
    }

    public void SetGridPosition(int x, int y)
    {
        gridPosition = new Vector2Int(x, y);
    }

}
