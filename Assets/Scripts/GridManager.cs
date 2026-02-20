using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public Vector3 playerPosition;
    public int rows = 5;          // Total rows (front to back)
    public int columns = 3;       // Total columns (left to right)
    private float cellSize = 3f;   // Spacing between tiles
    private Vector3 gridOrigin = new Vector3(0, 0, 0); // Front-left corner of grid
    private float gridCellOffsetY = -1.5f; // Visual offset below gameplay positions

    [Header("Prefabs")]
    public GameObject gridCellPrefab; // Should have centered pivot

    [Header("Pooling")]
    public int initialPoolSize = 15; // 5x3 grid
    private List<GameObject> gridCellPool = new List<GameObject>();

    [Header("Grid Data")]
    public List<Vector3> gridTiles = new List<Vector3>(); // Center points of cells
  

    void Awake()
    {
        Instance = this;
        CalculateGridCenters();
        InitializePool();
        CreateGridVisuals();
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM("BattleTheme1");
    }

    private void Update()
    {
        
    }

    void CalculateGridCenters()
    {
        gridTiles.Clear();
        float halfCell = cellSize * 0.5f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate center points of each cell
                Vector3 centerPos = new Vector3(
                    gridOrigin.x + (col * cellSize) + halfCell,
                    0,
                    gridOrigin.z + (row * cellSize) + halfCell
                );
                gridTiles.Add(centerPos);
            }
        }
    }

    void CreateGridVisuals()
    {
        foreach (Vector3 pos in gridTiles)
        {
            GameObject cell = GetPooledCell();
            cell.transform.position = new Vector3(
                pos.x,
                pos.y + gridCellOffsetY,
                pos.z
            );
            cell.SetActive(true);
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject cell = Instantiate(gridCellPrefab, Vector3.zero, Quaternion.identity);
            cell.transform.SetParent(transform);
            cell.SetActive(false);
            gridCellPool.Add(cell);
        }
    }

    GameObject GetPooledCell()
    {
        foreach (GameObject cell in gridCellPool)
        {
            if (!cell.activeInHierarchy) return cell;
        }

        GameObject newCell = Instantiate(gridCellPrefab, transform);
        gridCellPool.Add(newCell);
        return newCell;
    }

    public int GetPlayerColumn(Vector3 position)
    {
        
        float xPos = position.x;
        return Mathf.FloorToInt((xPos - gridOrigin.x) / cellSize);
    }


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

    public int GetColumnIndex(Vector3 position)
    {
        float relativeX = position.x - gridOrigin.x;
        int column = Mathf.FloorToInt(relativeX / cellSize);
        return Mathf.Clamp(column, 0, columns - 1);
    }

    public int GetIndex(int row, int column)
    {
        return row * columns + column;
    }

    public GridPosition GetGridPositionFromWorld(Vector3 worldPos)
    {
        int column = GetColumnIndex(worldPos);

        float relativeZ = worldPos.z - gridOrigin.z;
        int row = Mathf.FloorToInt(relativeZ / cellSize);
        row = Mathf.Clamp(row, 0, rows - 1);

        return new GridPosition(row, column);
    }

    public GridPosition GetGridPositionFromIndex(int index)
    {
        int row = index / columns;
        int column = index % columns;
        return new GridPosition(row, column);
    }

    public Vector3 GetWorldPosition(int row, int column)
    {
        int index = GetIndex(row, column);
        return gridTiles[index];
    }

    public void ClearGrid()
    {
        foreach (GameObject cell in gridCellPool)
        {
            cell.SetActive(false);
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (Vector3 pos in gridTiles)
        {
            Gizmos.DrawWireCube(pos, Vector3.one * 0.5f);
        }
    }
}

public struct GridPosition
{
    public int row;
    public int column;

    public GridPosition(int row, int column)
    {
        this.row = row;
        this.column = column;
    }
}