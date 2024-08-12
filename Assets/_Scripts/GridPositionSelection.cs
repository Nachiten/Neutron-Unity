using System;
using UnityEngine;

public class GridPositionSelection : MonoBehaviour
{
    public Action<GridPosition> OnGridPositionHovered;
    public Action<GridPosition> OnGridPositionUnhovered;
    
    public Action<GridPosition> OnGridPositionSelected;
    public Action<GridPosition> OnGridPositionUnselected;
    
    private GridPosition hoveredGridPosition = GridPosition.Null;
    private GridPosition selectedGridPosition = GridPosition.Null;
    
    private void Update()
    {
        HandleHovering();
        HandleSelection();
    }
    
    private void HandleHovering()
    {
        GridPosition newHoveredGridPosition = MouseWorldVisual.GetMouseGridPosition();

        if (!LevelGrid.Instance.GridPosIsValid(newHoveredGridPosition))
        {
            if (hoveredGridPosition != GridPosition.Null)
                UnhoverPosition();
            
            return;
        }
        
        if (hoveredGridPosition == newHoveredGridPosition)
            return;
        
        HoverPosition(newHoveredGridPosition);
    }
    
    private void UnhoverPosition()
    {
        HoverPosition(GridPosition.Null);
    }
    
    private void HoverPosition(GridPosition gridPosition)
    {
        // If there is a previously hovered grid position, unhover it
        if (hoveredGridPosition != GridPosition.Null)
            OnGridPositionUnhovered?.Invoke(hoveredGridPosition);
        
        hoveredGridPosition = gridPosition;
        
        // If the new hovered grid position is not null, hover it
        if (hoveredGridPosition != GridPosition.Null)
            OnGridPositionHovered?.Invoke(hoveredGridPosition);
    }
    
    private void HandleSelection()
    {
        if (!InputManager.Instance.WasPrimaryActionPerformedThisFrame())
            return;
        
        GridPosition newSelectedGridPosition = MouseWorldVisual.GetMouseGridPosition();
        
        if (!LevelGrid.Instance.GridPosIsValid(newSelectedGridPosition))
            return;

        if (selectedGridPosition == newSelectedGridPosition)
        { 
            UnselectPosition();
            return;
        }
        
        SelectPosition(newSelectedGridPosition);
    }
    
    private void UnselectPosition()
    {
        SelectPosition(GridPosition.Null);
    }
    
    private void SelectPosition(GridPosition gridPosition)
    {
        // If there is a previously hovered grid position, unhover it
        if (selectedGridPosition != GridPosition.Null)
            OnGridPositionUnselected?.Invoke(selectedGridPosition);
        
        selectedGridPosition = gridPosition;
        
        // If the new hovered grid position is not null, hover it
        if (selectedGridPosition != GridPosition.Null)
            OnGridPositionSelected?.Invoke(selectedGridPosition);
    }
}
