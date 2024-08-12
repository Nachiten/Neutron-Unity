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
            if (hoveredGridPosition)
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
        if (hoveredGridPosition)
            OnGridPositionUnhovered?.Invoke(hoveredGridPosition);
        
        hoveredGridPosition = gridPosition;
        
        // If the new hovered grid position is not null, hover it
        if (hoveredGridPosition)
            OnGridPositionHovered?.Invoke(hoveredGridPosition);
    }
    
    private void HandleSelection()
    {
        if (!InputManager.Instance.WasPrimaryActionReleasedThisFrame())
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
        if (selectedGridPosition)
            OnGridPositionUnselected?.Invoke(selectedGridPosition);
        
        selectedGridPosition = gridPosition;
        
        // If the new hovered grid position is not null, hover it
        if (selectedGridPosition)
            OnGridPositionSelected?.Invoke(selectedGridPosition);
    }
}
