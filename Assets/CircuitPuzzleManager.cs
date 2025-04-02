using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzleManager : MonoBehaviour
{
    public int gridWidth, gridHeight; // Define the grid size
    private WireTileHandling[,] wireGrid; // 2D array for storing tiles

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
    }

    public void RegisterTile(WireTileHandling tile, int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            wireGrid[x, y] = tile;
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
}
