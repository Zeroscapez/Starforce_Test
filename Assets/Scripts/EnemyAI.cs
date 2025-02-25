using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Grid Settings")]
    public float moveInterval = 3f; // Time between moves
    public int minRow = 1; // For enemies, front row is Row 0 (player's row) so start at Row 1
    public int maxRow = 4; // Adjust based on your grid size

    private float timer;
    private int currentTileIndex;

    // Expose the current tile index publicly.
    public int CurrentTileIndex { get { return currentTileIndex; } }

    // Returns grid position as (column, row)
    public Vector2Int GetGridPosition()
    {
        int columns = GridManager.Instance.columns;
        int row = currentTileIndex / columns;
        int col = currentTileIndex % columns;
        //Debug.Log(col + "," + row);
        return new Vector2Int(col, row);

    }

    void Start()
    {
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

    int GetRandomValidTileIndex()
    {
        int columns = GridManager.Instance.columns;
        int rows = GridManager.Instance.rows;
        int randomRow = Random.Range(minRow, maxRow + 1);
        int randomColumn = Random.Range(0, columns);
        int index = (randomRow * columns) + randomColumn;
        return index;
    }

    void SnapToTile(int index)
    {
        if (index >= 0 && index < GridManager.Instance.gridTiles.Count)
        {
            transform.position = GridManager.Instance.gridTiles[index];
        }
    }
}
