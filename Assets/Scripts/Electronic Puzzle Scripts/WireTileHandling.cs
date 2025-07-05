using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Directions used to define wire connections.
/// </summary>
public enum Direction
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3
}

/// <summary>
/// Represents a directional connection for a wire tile.
/// </summary>
[Serializable]
public class DirectionConnection
{
    public Direction direction;      // The direction this connection is facing
    public bool isConnected;         // Whether this direction is actively connected
}

/// <summary>
/// Handles dragging, rotating, and wire state management for a single wire tile.
/// </summary>
public class WireTileHandling : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// The tile's position in the puzzle grid.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public Vector2Int gridPosition { get; private set; }

    /// <summary>
    /// Reference to the puzzle manager that manages the overall circuit.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public CircuitPuzzleManager puzzleManager;

    /// <summary>
    /// Color when the wire is turned on.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public Color onColor = Color.green;

    public Sprite onImage;

    /// <summary>
    /// Color when the wire is turned off.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public Color offColor = Color.red;

    public Sprite offImage;

    /// <summary>
    /// Indicates whether the wire is currently powered.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public bool isWireOn = false;

    /// <summary>
    /// Reference to the image component representing the tile visually.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public Image image;

    private TileSlot initialParentScript;   // Cached reference to the parent slot's script

    /// <summary>
    /// Original parent of the tile before dragging.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    [HideInInspector] public Transform initialParent;

    /// <summary>
    /// Parent the tile will return to after dragging.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    [HideInInspector] public Transform parentAfterDrag;

    /// <summary>
    /// List of directional connections for the tile (visible in inspector).
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    [SerializeField]
    public List<DirectionConnection> connectionsList = new List<DirectionConnection>
    {
        new DirectionConnection { direction = Direction.Top, isConnected = false },
        new DirectionConnection { direction = Direction.Right, isConnected = false },
        new DirectionConnection { direction = Direction.Bottom, isConnected = false },
        new DirectionConnection { direction = Direction.Left, isConnected = false }
    };

    private int rotationState = 0; // Tracks the number of 90-degree rotations (0-3)

    /// <summary>
    /// Initializes the tile, caching references and setting color states.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    private void Start()
    {
        initialParent = transform.parent;
        initialParentScript = initialParent.GetComponent<TileSlot>();
        UpdateWireColor();

        puzzleManager = GetComponentInParent<CircuitPuzzleManager>();

        if (puzzleManager == null)
        {
            puzzleManager = FindObjectOfType<CircuitPuzzleManager>();
        }
    }

    /// <summary>
    /// Called when dragging starts.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Dragging is disabled for this slot.");
            return;
        }

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        if (puzzleManager != null)
        {
            Vector2Int oldPos = gridPosition;
            puzzleManager.UpdateWireGrid(oldPos.x, oldPos.y, null);
            puzzleManager.LightUpConnectedWires();
        }

        TurnOff();
    }

    /// <summary>
    /// Called while dragging the tile.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void OnDrag(PointerEventData eventData)
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Dragging is disabled for this slot.");
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    /// <summary>
    /// Called when dragging ends.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Dragging is disabled for this slot.");
            return;
        }

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        TileSlot newSlot = parentAfterDrag.GetComponent<TileSlot>();
        if (newSlot != null)
        {
            gridPosition = new Vector2Int(newSlot.x, newSlot.y);

            if (puzzleManager != null)
            {
                puzzleManager.UpdateWireGrid(newSlot.x, newSlot.y, this);
                puzzleManager.LightUpConnectedWires();
            }
        }
    }

    /// <summary>
    /// Rotates the tile clockwise and updates the directional connections.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void RotateTile()
    {
        if (initialParentScript != null && !initialParentScript.isEditable)
        {
            Debug.Log("Rotation is NOT Allowed.");
            return;
        }

        rotationState = (rotationState + 1) % 4;
        transform.Rotate(0, 0, -90);

        bool lastConnection = connectionsList[3].isConnected;

        for (int i = 3; i > 0; i--)
        {
            connectionsList[i].isConnected = connectionsList[i - 1].isConnected;
        }
        connectionsList[0].isConnected = lastConnection;

        if (puzzleManager != null)
        {
            puzzleManager.LightUpConnectedWires();
        }
    }

    /// <summary>
    /// Updates the wire color based on its on/off state.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    private void UpdateWireColor()
    {
        if (image != null)
        {
            image.color = isWireOn ? onColor : offColor;
        }
    }

    /// <summary>
    /// Turns the wire on and updates its visual state.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void TurnOn()
    {
        isWireOn = true;
        UpdateWireColor();

        if (onImage != null)
        {
            image.sprite = onImage;
        }
    }

    /// <summary>
    /// Turns the wire off and updates its visual state.
    /// </summary>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void TurnOff()
    {
        isWireOn = false;
        UpdateWireColor();

        if (offImage != null)
        {
            image.sprite = offImage;
        }
    }

    /// <summary>
    /// Returns a list of directions that are currently connected.
    /// </summary>
    /// <returns>List of connected directions.</returns>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
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

    /// <summary>
    /// Sets the wire’s grid position manually.
    /// </summary>
    /// <param name="x">Grid X position.</param>
    /// <param name="y">Grid Y position.</param>
    /// <remarks>
    /// Maintained by: Michael Edems-Eze
    /// </remarks>
    public void SetGridPosition(int x, int y)
    {
        gridPosition = new Vector2Int(x, y);
    }
}
