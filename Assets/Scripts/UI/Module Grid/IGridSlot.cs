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

    /// <summary>
    ///     Highlights the grid slot and any additional grid slots in gridpositions
    /// </summary>
    /// <param name="gridPositions"></param>
    public void Highlight(List<Vector2Int> gridPositions);

    /// <summary>
    ///     Dehighlights the grid slot and other grid slots that were highlighted by Highlight
    /// </summary>
    public void DeHighlight();
}