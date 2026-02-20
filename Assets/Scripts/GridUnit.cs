using UnityEngine;
using System.Collections;


	public class GridUnit: MonoBehaviour
	{

    public GridPosition CurrentGridPosition;

    public void SetGridPosition(GridPosition newPos)
    {
        CurrentGridPosition = newPos;

        transform.position = GridManager.Instance.GetWorldPosition(newPos.row, newPos.column);
    }
}
