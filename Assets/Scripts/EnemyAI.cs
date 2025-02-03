using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Grid Settings")]
    public float moveInterval = 3f; // Time between moves
    public int minRow = 1; // Start at Row 1 (front row is Row 0 for the player)
    public int maxRow = 4; // Adjust based on your grid size

    private float timer;
    private int currentTileIndex;

    void Start()
    {
        // Start at a random valid tile
        currentTileIndex = GetRandomValidTileIndex();
        SnapToTile(currentTileIndex);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            currentTileIndex = GetRandomValidTileIndex();
            SnapToTile(currentTileIndex);
            timer = 0;
        }
    }

    // Get a random tile index within allowed rows/columns
    int GetRandomValidTileIndex()
    {
        int columns = GridManager.Instance.columns;
        int rows = GridManager.Instance.rows;

        // Randomly pick a row (between minRow and maxRow)
        int randomRow = Random.Range(minRow, maxRow + 1);
        // Randomly pick a column (0 to 2 for 3 columns)
        int randomColumn = Random.Range(0, columns);

        // Convert row/column to gridTiles index (row-major order)
        int index = (randomRow * columns) + randomColumn;
        return index;
    }

    // Move smoothly or instantly to the target tile
    void SnapToTile(int index)
    {
        if (index >= 0 && index < GridManager.Instance.gridTiles.Count)
        {
            transform.position = GridManager.Instance.gridTiles[index];
        }
    }
}