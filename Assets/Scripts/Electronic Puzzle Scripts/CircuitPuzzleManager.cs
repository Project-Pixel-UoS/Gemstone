using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the logic for a grid-based circuit puzzle,
/// including tile registration, DFS-based connection checking,
/// and win condition verification.
/// </summary>
public class CircuitPuzzleManager : MonoBehaviour
{
    /// <summary>
    /// Width of the wire grid.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public int gridWidth;

    /// <summary>
    /// Height of the wire grid.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public int gridHeight;

    /// <summary>
    /// 2D array storing wire tiles.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public WireTileHandling[,] wireGrid;

    /// <summary>
    /// Reference to the initial powered wire tile.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public WireTileHandling powerSource;

    /// <summary>
    /// Tiles that must be powered for the puzzle to be considered solved.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public WireTileHandling[] endTiles;

    /// <summary>
    /// UI manager to handle puzzle win animations.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public PanelAnimator puzzleUIManager;

    /// <summary>
    /// Initializes the grid, registers tiles, and attempts to light up the circuit.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    void Start()
    {
        wireGrid = new WireTileHandling[gridWidth, gridHeight];

        WireTileHandling[] tiles = GetComponentsInChildren<WireTileHandling>();

        int count = 0;
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (count < tiles.Length)
                {
                    RegisterTile(tiles[count], x, y);

                    GameObject slotObject = transform.GetChild(count).gameObject;
                    TileSlot slot = slotObject.GetComponent<TileSlot>();
                    slot.x = x;
                    slot.y = y;

                    count++;
                }
            }
        }

        LightUpConnectedWires();

        Debug.Log("Grid position of first tile: " + wireGrid[0, 0].gridPosition);
        Debug.Log("There are this many Tiles all in all: " + count);
    }

    /// <summary>
    /// Registers a tile to a specific grid position.
    /// </summary>
    /// <param name="tile">The wire tile to register.</param>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public void RegisterTile(WireTileHandling tile, int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            wireGrid[x, y] = tile;
            tile.SetGridPosition(x, y);
        }
        else
        {
            Debug.LogWarning($"Tile at ({x},{y}) is out of bounds!");
        }
    }

    /// <summary>
    /// Gets a wire tile at a specific grid position.
    /// </summary>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <returns>The tile at the position or null.</returns>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public WireTileHandling GetTileAtPosition(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return wireGrid[x, y];
        }
        return null;
    }

    /// <summary>
    /// Lights up all tiles connected to the power source using DFS.
    /// </summary>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public void LightUpConnectedWires()
    {
        if (powerSource == null) return;

        HashSet<WireTileHandling> visited = new HashSet<WireTileHandling>();
        DFS(powerSource, visited);

        foreach (WireTileHandling tile in wireGrid)
        {
            if (tile != null && !visited.Contains(tile))
            {
                tile.TurnOff();
            }
        }

        CheckIfSolved(endTiles);
    }

    /// <summary>
    /// Adds a tile to the grid at a specific position.
    /// </summary>
    /// <param name="tile">The tile to add.</param>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public void AddTileAt(WireTileHandling tile, int x, int y)
    {
        if (IsWithinBounds(new Vector2Int(x, y)))
        {
            wireGrid[x, y] = tile;
            tile.SetGridPosition(x, y);
        }
    }

    /// <summary>
    /// Removes a tile from the grid at the given position.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public void RemoveTileAt(int x, int y)
    {
        if (IsWithinBounds(new Vector2Int(x, y)))
        {
            wireGrid[x, y] = null;
        }
    }

    /// <summary>
    /// Updates the wire grid with a new tile at a specific position.
    /// </summary>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <param name="newTile">The tile to insert.</param>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public void UpdateWireGrid(int x, int y, WireTileHandling newTile)
    {
        if (IsWithinBounds(new Vector2Int(x, y)))
        {
            wireGrid[x, y] = newTile;
        }

        if (newTile != null)
        {
            newTile.SetGridPosition(x, y);
        }
    }

    /// <summary>
    /// Recursive DFS algorithm to turn on and track connected wires.
    /// </summary>
    /// <param name="wire">Current tile being visited.</param>
    /// <param name="visited">Set of already visited tiles.</param>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    private void DFS(WireTileHandling wire, HashSet<WireTileHandling> visited)
    {
        if (wire == null || visited.Contains(wire)) return;

        wire.TurnOn();
        visited.Add(wire);

        Vector2Int pos = wire.gridPosition;

        foreach (Direction dir in wire.GetConnectedDirections())
        {
            Vector2Int neighborPos = pos + GetDirectionOffset(dir);

            if (IsWithinBounds(neighborPos))
            {
                WireTileHandling neighborWire = wireGrid[neighborPos.x, neighborPos.y];

                if (neighborWire != null && neighborWire.GetConnectedDirections().Contains(OppositeDirection(dir)))
                {
                    DFS(neighborWire, visited);
                }
            }
        }
    }

    /// <summary>
    /// Checks whether a given grid position is within the grid bounds.
    /// </summary>
    /// <param name="pos">The position to check.</param>
    /// <returns>True if valid, false otherwise.</returns>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    /// <summary>
    /// Returns the Vector2 offset for a given direction. This vector2 will be applied to the current tile position to get the next tile in DFS.
    /// </summary>
    /// <param name="direction">The direction to convert.</param>
    /// <returns>Offset vector.</returns>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    private Vector2Int GetDirectionOffset(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top: return new Vector2Int(0, -1);
            case Direction.Right: return new Vector2Int(1, 0);
            case Direction.Bottom: return new Vector2Int(0, 1);
            case Direction.Left: return new Vector2Int(-1, 0);
            default: return Vector2Int.zero;
        }
    }

    /// <summary>
    /// Returns the opposite direction of the one passed in.
    /// </summary>
    /// <param name="dir">The original direction.</param>
    /// <returns>The opposite direction.</returns>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    private Direction OppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Top: return Direction.Bottom;
            case Direction.Right: return Direction.Left;
            case Direction.Bottom: return Direction.Top;
            case Direction.Left: return Direction.Right;
            default: return Direction.Top;
        }
    }

    /// <summary>
    /// Checks if all required end tiles are powered and triggers win condition if true.
    /// </summary>
    /// <param name="requiredTiles">Array of required powered tiles.</param>
    /// <remarks>Maintained by: Michael Edems-Eze</remarks>
    public void CheckIfSolved(WireTileHandling[] requiredTiles)
    {
        bool allConnected = true;

        foreach (WireTileHandling requiredTile in requiredTiles)
        {
            if (!requiredTile.isWireOn)
            {
                allConnected = false;
                break;
            }
        }

        if (allConnected)
        {
            puzzleUIManager.DisplayWinMessage();
            // Additional win logic can go here
        }
    }
}
