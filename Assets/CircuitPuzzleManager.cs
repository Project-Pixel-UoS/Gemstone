using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzleManager : MonoBehaviour
{
    public int gridWidth, gridHeight; // Define the grid size
    public WireTileHandling[,] wireGrid; // 2D array for storing tiles
    public WireTileHandling powerSource; // Assign the starting wire with power

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

        // Turn off all unvisited wires
        foreach (WireTileHandling tile in wireGrid)
        {
            if (tile != null && !visited.Contains(tile))
            {
                tile.TurnOff();
            }
        }
    }

    private void DFS(WireTileHandling wire, HashSet<WireTileHandling> visited)
    {
        if (wire == null || visited.Contains(wire)) return;

        wire.TurnOn();
        visited.Add(wire);

        Vector2Int pos = wire.gridPosition;
        foreach (Direction dir in wire.GetConnectedDirections()) // Get valid connections
        {
            Vector2Int neighborPos = pos + GetDirectionOffset(dir);
            if (IsWithinBounds(neighborPos))
            {
                DFS(wireGrid[neighborPos.x, neighborPos.y], visited);
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
            case Direction.Top: return new Vector2Int(0, 1);
            case Direction.Right: return new Vector2Int(1, 0);
            case Direction.Bottom: return new Vector2Int(0, -1);
            case Direction.Left: return new Vector2Int(-1, 0);
            default: return Vector2Int.zero;
        }
    }

}
