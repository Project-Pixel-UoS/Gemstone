using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzleManager : MonoBehaviour
{
    public int gridWidth, gridHeight; // Define the grid size
    public WireTileHandling[,] wireGrid; // 2D array for storing tiles
    public WireTileHandling powerSource; // Assign the starting wire with power
    public WireTileHandling[] endTiles; // Assign the starting wire with power

    public PanelAnimator puzzleUIManager;

    void Start()
    {
        wireGrid = new WireTileHandling[gridWidth, gridHeight]; // Initialize grid

        WireTileHandling[] tiles = GetComponentsInChildren<WireTileHandling>(); // Only check children

        int count = 0; // Track how many tiles have been assigned
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (count < tiles.Length) // Ensure we don't go out of bounds
                {
                    RegisterTile(tiles[count], x, y);
                    count++;

                    // Get the GameObject of the current tile slot
                    GameObject slotObject = transform.GetChild(count).gameObject;

                    // Get the TileSlot component
                    TileSlot slot = slotObject.GetComponent<TileSlot>();
                    slot.x = x;
                    slot.y = y;
                }
            }
        }

        LightUpConnectedWires();
        Debug.Log("Grid position of first tile: " + wireGrid[0, 0].gridPosition);
        Debug.Log("There are this many Tiles all in all: " + count);
    }

    public void RegisterTile(WireTileHandling tile, int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            wireGrid[x, y] = tile;
            tile.SetGridPosition(x, y); // Assigns the grid position
        }
        else
        {
            Debug.LogWarning($"Tile at ({x},{y}) is out of bounds!");
        }
    }

    public WireTileHandling GetTileAtPosition(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return wireGrid[x, y];
        }
        return null;
    }

    public void LightUpConnectedWires()
    {
        
        if (powerSource == null) return;

        HashSet<WireTileHandling> visited = new HashSet<WireTileHandling>();
        DFS(powerSource, visited);

        // Debug log to print the size of visited and wireGrid
        Debug.Log($"Visited size: {visited.Count}");
        Debug.Log($"WireGrid size: {gridWidth} x {gridHeight}");

        // Turn off all unvisited wires
        foreach (WireTileHandling tile in wireGrid)
        {
            if (tile != null && !visited.Contains(tile))
            {
                tile.TurnOff();
            }
        }

        CheckIfSolved(endTiles);

    }

    // Add a tile to the grid
    public void AddTileAt(WireTileHandling tile, int x, int y)
    {
        if (IsWithinBounds(new Vector2Int(x, y)))
        {
            wireGrid[x, y] = tile;
            tile.SetGridPosition(x, y);
        }
    }

    // Remove a tile from the grid
    public void RemoveTileAt(int x, int y)
    {
        if (IsWithinBounds(new Vector2Int(x, y)))
        {
            wireGrid[x, y] = null;
        }
    }

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

    private void DFS(WireTileHandling wire, HashSet<WireTileHandling> visited)
    {
        if (wire == null || visited.Contains(wire)) return;

        // Debug log when we visit a tile
        Debug.Log($"Visiting tile at position: {wire.gridPosition}");

        wire.TurnOn();
        visited.Add(wire);

        Vector2Int pos = wire.gridPosition;

        // Loop through all connected directions of the wire
        foreach (Direction dir in wire.GetConnectedDirections())
        {
            // Log which direction we're currently considering
            Debug.Log($"Checking direction: {dir} from tile at position: {wire.gridPosition}");

            Vector2Int neighborPos = pos + GetDirectionOffset(dir);

            if (IsWithinBounds(neighborPos))
            {
                // Get the neighboring tile at the calculated position
                WireTileHandling neighborWire = wireGrid[neighborPos.x, neighborPos.y];

                // Check if the neighboring tile has a valid connection for the current direction
                if (neighborWire != null && neighborWire.GetConnectedDirections().Contains(OppositeDirection(dir)))
                {
                    // Log that we are going to recursively call DFS for the valid neighboring tile
                    Debug.Log($"Valid connection found to neighbor at {neighborPos}. Recursively calling DFS.");

                    // Recursively call DFS for the valid neighboring tile
                    DFS(neighborWire, visited);
                }
                else
                {
                    // Log if no valid connection is found
                    Debug.Log($"No valid connection for direction: {dir} at position: {wire.gridPosition} to neighbor at {neighborPos}");
                }
            }
            else
            {
                // Log if the neighboring position is out of bounds
                Debug.Log($"Neighbor position {neighborPos} is out of bounds.");
            }
        }
    }


    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

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

    private Direction OppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Top:
                return Direction.Bottom;
            case Direction.Right:
                return Direction.Left;
            case Direction.Bottom:
                return Direction.Top;
            case Direction.Left:
                return Direction.Right;
            default:
                return Direction.Top; // Default to Top if something goes wrong
        }
    }

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
            // Trigger any win condition logic here
        }
    }

}
