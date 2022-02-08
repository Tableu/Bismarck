using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     An interface for a slot in an inventory/customization grid.
///     Used to allow highlighting grid spaces across different grids
/// </summary>
public interface IGridSlot
{
    public Vector2Int GetPosition();
    public void SetColor(Color color);
    public void Enter(List<Vector2Int> gridPositions);
    public void Exit();
}