using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class GridSystem<TGridObject>
{
    private readonly float cellSize;

    private readonly TGridObject[,] gridObjects;
    private readonly int height;
    private readonly int width;
    private readonly int floor;
    private readonly float floorHeight;

    public GridSystem(int width, int height, float cellSize, int floor, float floorHeight,
        Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.floor = floor;
        this.floorHeight = floorHeight;
        
        gridObjects = new TGridObject[width, height];
        
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
            gridObjects[x, y] = createGridObject(this, new GridPosition(x, y, floor));
    }

    public Vector3 GetWorldPos(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, gridPosition.y, 0) * cellSize + new Vector3(0, gridPosition.floor * floorHeight, 0);
    }

    public GridPosition GetGridPos(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.z / cellSize);

        return new GridPosition(x, y, floor);
    }

    public void CreateDebugObjects(Transform debugPrefab, Transform debugPrefabParent)
    {
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            GridPosition gridPosition = new(x, y, floor);
            
            Transform debugTransform = Object.Instantiate(
                debugPrefab, 
                GetWorldPos(gridPosition), 
                Quaternion.identity,
                debugPrefabParent);
    
            GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
            gridDebugObject.SetGridObject(GetGridObject(gridPosition));
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[gridPosition.x, gridPosition.y];
    }

    public bool GridPosIsValid(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.x < width &&
               gridPosition.y >= 0 &&
               gridPosition.y < height &&
               gridPosition.floor == floor;
    }

    public int GetWidth() => width;
    public int GetHeight() => height;
}