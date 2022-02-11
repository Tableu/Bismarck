using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Provides info on the grid slot and highlights/dehighlights it
/// </summary>
public class ModuleGridSlot : MonoBehaviour, IGridSlot
{
    public Vector2Int Position;
    public ModuleGridView ModuleGridView;
    [SerializeField] private Color HoverColor;
    [SerializeField] private Image _image;
    private List<Vector2Int> _currentGridPositions;

    public Vector2Int GetPosition()
    {
        return Position;
    }
    public void SetColor(Color color)
    {
        _image.color = color;
    }

    public void Highlight(List<Vector2Int> gridPositions)
    {
        _image.color = HoverColor;
        if (gridPositions == null || gridPositions.Count <= 0)
        {
            return;
        }

        foreach (Vector2Int pos in gridPositions)
        {
            ModuleGridSlot gridSlot = ModuleGridView.GetGridSlot(Position.y + pos.y, Position.x + pos.x);
            if (gridSlot != null)
            {
                gridSlot.SetColor(HoverColor);
            }
        }

        _currentGridPositions = gridPositions;
    }

    public void DeHighlight()
    {
        _image.color = new Color(1, 1, 1, 1f);
        if (_currentGridPositions == null)
        {
            return;
        }

        foreach (Vector2Int pos in _currentGridPositions)
        {
            ModuleGridSlot gridSlot = ModuleGridView.GetGridSlot(Position.y + pos.y, Position.x + pos.x);
            if (gridSlot != null)
            {
                gridSlot.SetColor(new Color(1, 1, 1, 1f));
            }
        }

        _currentGridPositions = null;
    }
}
