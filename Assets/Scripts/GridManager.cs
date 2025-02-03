using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int rows = 5;          // Total rows (front to back)
    public int columns = 3;       // Total columns (left to right)
    public float cellSize = 2f;   // Spacing between tiles
    public Vector3 gridOrigin = new Vector3(-2f, 0, 0); // Front-left corner (Row 0, Column 0)

    [Header("Grid Data")]
    public List<Vector3> gridTiles = new List<Vector3>(); // All tiles (indexed row-major)

    void Awake()
    {
        Instance = this;
        GenerateGrid();
    }

    // Generates the 5x3 grid
    void GenerateGrid()
    {
        gridTiles.Clear();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position (X: left/right, Z: front/back)
                Vector3 tilePos = gridOrigin + new Vector3(
                    col * cellSize,
                    0,
                    row * cellSize // Z increases for rows further back
                );
                gridTiles.Add(tilePos);
            }
        }
    }

    // Get the player's current column (0–2) based on X position
    public int GetPlayerColumn(Vector3 position)
    {
        float xPos = position.x;
        return Mathf.RoundToInt((xPos - gridOrigin.x) / cellSize);
    }

    // Get the nearest tile index for snapping
    public int GetNearestTileIndex(Vector3 position)
    {
        float minDistance = Mathf.Infinity;
        int nearestIndex = 0;

        for (int i = 0; i < gridTiles.Count; i++)
        {
            float distance = Vector3.Distance(position, gridTiles[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }
        return nearestIndex;
    }

    // Draw grid in the Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (Vector3 pos in gridTiles)
        {
            Gizmos.DrawWireCube(pos, Vector3.one * 0.5f);
        }
    }

    // GridManager.cs (additional methods)
    public int GetColumnIndex(Vector3 position)
    {
        float relativeX = position.x - gridOrigin.x;
        int column = Mathf.FloorToInt(relativeX / cellSize);
        return Mathf.Clamp(column, 0, columns - 1);
    }

    public List<EnemyHealth> enemies = new List<EnemyHealth>();

    public void RegisterEnemy(EnemyHealth enemy)
    {
        if (!enemies.Contains(enemy)) enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyHealth enemy)
    {
        if (enemies.Contains(enemy)) enemies.Remove(enemy);
    }

}