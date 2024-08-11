using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : Singleton<LevelGrid>
{
    private const float FLOOR_HEIGHT = 3f;

    public event Action<GridElement, GridPosition, GridPosition> OnAnyGridElementMovedGridPosition;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private Transform gridDebugObjectParent;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private List<GridSystem<GridObject>> gridSystems;

    protected override void Awake()
    {
        base.Awake();
        
        InitializeGridSystems();
        SetWalkableGridPositions();
    }

    private void InitializeGridSystems()
    {
        gridSystems = new List<GridSystem<GridObject>>();
        
        GridSystem<GridObject> gridSystem = new(width, height, cellSize, FLOOR_HEIGHT,
            (gridSystem, gridPosition) => new GridObject(gridSystem, gridPosition));

        gridSystem.CreateDebugObjects(gridDebugObjectPrefab, gridDebugObjectParent);
        gridSystems.Add(gridSystem);
    }

    private void SetWalkableGridPositions()
    {
        // By default, all grid positions are walkable

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            GridPosition gridPosition = new(x, y);
            
            Vector3 worldPosition = GetWorldPos(gridPosition);
            const float raycastOffsetDistance = 1f;

            // Check if there is an obstacle above the grid position
            bool obstaclesRaycast = Physics.Raycast(
                worldPosition + Vector3.down * raycastOffsetDistance,
                Vector3.up,
                raycastOffsetDistance * 2,
                obstaclesLayerMask);

            // If there is an obstacle, set the grid position as unwalkable
            if (obstaclesRaycast)
                GetGridObjectAtGridPos(gridPosition).SetIsWalkable(false);
        }
    }

    /// <summary>
    /// Add a GridElement to the grid at the given grid position
    /// </summary>
    /// <param name="gridPos"> The grid position to add the GridElement to </param>
    /// <param name="gridElement"> The GridElement to add to the grid </param>
    public void AddGridElementAtGridPos(GridPosition gridPos, GridElement gridElement)
    {
        GetGridObjectAtGridPos(gridPos).AddGridElement(gridElement);
    }

    /// <summary>
    /// Gets the list of units at the given grid position
    /// </summary>
    /// <param name="gridPos"> The grid position to get the list of units from </param>
    /// <returns> The list of units at the given grid position </returns>
    public List<GridElement> GetGridElementListAtGridPos(GridPosition gridPos)
    {
        return GetGridObjectAtGridPos(gridPos).GetGridElementList();
    }

    /// <summary>
    /// Remove a GridElement from the given grid position
    /// </summary>
    /// <param name="gridPos"> The grid position to remove the GridElement from </param>
    /// <param name="gridElement"> The GridElement to remove from the grid </param>
    private void RemoveGridElementAtGridPos(GridPosition gridPos, GridElement gridElement)
    {
        GetGridObjectAtGridPos(gridPos).RemoveGridElement(gridElement);
    }

    /// <summary>
    /// Move a GridElement from fromGridPos to toGridPos
    /// </summary>
    /// <param name="gridElement"> The GridElement to move </param>
    /// <param name="fromGridPos"> The origin grid position </param>
    /// <param name="toGridPos"> The destination grid position </param>
    public void MoveGridElementGridPos(GridElement gridElement, GridPosition fromGridPos, GridPosition toGridPos)
    {
        RemoveGridElementAtGridPos(fromGridPos, gridElement);
        AddGridElementAtGridPos(toGridPos, gridElement);

        OnAnyGridElementMovedGridPosition?.Invoke(gridElement, fromGridPos, toGridPos);
    }

    /// <summary>
    /// Get if the given grid position is valid
    /// </summary>
    /// <param name="gridPos"> The grid position to check </param>
    /// <returns> True if the grid position is valid </returns>
    private bool GridPosIsValid(GridPosition gridPos)
    {
        return GetGridSystem().GridPosIsValid(gridPos);
    }

    /// <summary>
    /// Get if the given grid position is walkable
    /// </summary>
    /// <param name="gridPos"> The grid position to check </param>
    /// <returns> True if the grid position is walkable </returns>
    private bool GridPosIsWalkable(GridPosition gridPos)
    {
        return GetGridObjectAtGridPos(gridPos).GetIsWalkable();
    }

    /// <summary>
    /// Get if the given grid position has any GridElement
    /// </summary>
    /// <param name="gridPos"> The grid position to check </param>
    /// <returns> True if the grid position has any GridElement </returns>
    public bool GridPosHasAnyGridElement(GridPosition gridPos)
    {
        return GetGridObjectAtGridPos(gridPos).HasAnyGridElement();
    }

    /// <summary>
    /// Get the first GridElement at the given grid position
    /// </summary>
    /// <param name="gridPos"> The grid position to get the GridElement from </param>
    /// <returns> The first GridElement at the given grid position </returns>
    public GridElement GetGridElementAtGridPos(GridPosition gridPos)
    {
        return GetGridObjectAtGridPos(gridPos).GetGridElement();
    }

    /// <summary>
    /// Get the grid object at the given grid position
    /// </summary>
    /// <param name="gridPos"> The grid position to get the grid object from </param>
    /// <returns> The grid object at the given grid position </returns>
    private GridObject GetGridObjectAtGridPos(GridPosition gridPos)
    {
        return GetGridSystem().GetGridObject(gridPos);
    }

    /// <summary>
    /// Get the floor from the given world position
    /// </summary>
    /// <param name="worldPos"> The world position to get the floor from </param>
    /// <returns> The floor from the given world position </returns>
    private int GetFloorFromWorldPos(Vector3 worldPos)
    {
        return Mathf.RoundToInt(worldPos.y / FLOOR_HEIGHT);
    }

    /// <summary>
    /// Get the grid position from the given world position
    /// </summary>
    /// <param name="worldPos"> The world position to get the grid position from </param>
    /// <returns> The grid position from the given world position </returns>
    public GridPosition GetGridPos(Vector3 worldPos)
    {
        return GetGridSystem(GetFloorFromWorldPos(worldPos)).GetGridPos(worldPos);
    }

    /// <summary>
    /// Get the world position from the given grid position
    /// </summary>
    /// <param name="gridPos"> The grid position to get the world position from </param>
    /// <returns> The world position from the given grid position </returns>
    public Vector3 GetWorldPos(GridPosition gridPos)
    {
        return GetGridSystem().GetWorldPos(gridPos);
    }
    
    public bool ValidGridPosToMove(GridPosition gridPos)
    {
        return GridPosIsValid(gridPos) && GridPosIsWalkable(gridPos);
    }

    private GridSystem<GridObject> GetGridSystem(int index = 0) => gridSystems[index];
    public int GetWidth() => GetGridSystem().GetWidth();
    public int GetHeight() => GetGridSystem().GetHeight(); 
    
    public float GetCellSize() => cellSize;
}