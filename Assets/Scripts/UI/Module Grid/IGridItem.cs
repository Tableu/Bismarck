using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Interface for items in an inventory/customization grid.
/// </summary>
public interface IGridItem
{
    public List<Vector2Int> GetItemPositions();
}