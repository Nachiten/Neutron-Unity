using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private Transform gridSystemVisualSingleParent;
    [SerializeField] private GridPositionSelection gridPositionSelection;

    private GridSystemVisualSingle[,] gridSystemVisualSingles;
    private Dictionary<GridVisualType, Color> gridVisualTypeColors;

    private void Awake()
    { 
        List<GridVisualTypeColor> gridVisualTypeColorList = Resources.Load<GridVisualTypeColorsSO>(nameof(GridVisualTypeColorsSO)).gridVisualTypeColors;
        
        gridVisualTypeColors = new Dictionary<GridVisualType, Color>();
        
        foreach (GridVisualTypeColor gridVisualTypeColor in gridVisualTypeColorList)
            gridVisualTypeColors.Add(gridVisualTypeColor.gridVisualType, gridVisualTypeColor.color);
    }

    private void Start()
    {
        InstantiateGridSystemVisuals();

        gridPositionSelection.OnGridPositionHovered += OnGridPositionHovered;
        gridPositionSelection.OnGridPositionUnhovered += OnGridPositionUnhovered;
        gridPositionSelection.OnGridPositionSelected += OnGridPositionSelected;
        gridPositionSelection.OnGridPositionUnselected += OnGridPositionUnselected;
    }
    
    private void OnGridPositionHovered(GridPosition hoveredGridPos)
    {
        SetGridPositionsHovered(new List<GridPosition> {hoveredGridPos}, true);
    }

    private void OnGridPositionUnhovered(GridPosition gridPos)
    {
        SetGridPositionsHovered(new List<GridPosition> {gridPos}, false);
    }
    
    private void OnGridPositionSelected(GridPosition selectedGridPos)
    {
        Debug.Log("[GridSystemVisual] Selected: " + selectedGridPos);
        ColorGridPositions(new List<GridPosition> {selectedGridPos}, GridVisualType.Red);
    }
    
    private void OnGridPositionUnselected(GridPosition gridPos)
    {
        Debug.Log("[GridSystemVisual] Unselected: " + gridPos);
        ResetGridPositionColor(gridPos);
    }

    private void InstantiateGridSystemVisuals()
    {
        int width = LevelGrid.Instance.GetWidth();
        int height = LevelGrid.Instance.GetHeight();

        gridSystemVisualSingles = new GridSystemVisualSingle[width, height];

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 worldPos = LevelGrid.Instance.GetWorldPos(new GridPosition(x, y));
            
            Transform gridSystemVisual =
                Instantiate(gridSystemVisualSinglePrefab, 
                    worldPos, 
                    Quaternion.identity, 
                    gridSystemVisualSingleParent);
                
            gridSystemVisualSingles[x, y] = gridSystemVisual.GetComponent<GridSystemVisualSingle>();
            gridSystemVisualSingles[x, y].SetDefaultColor(GetGridVisualTypeColor(GridVisualType.White));
        }
    }
    
    private void ColorGridPositions(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        gridPositions.ForEach(gridPosition =>
            gridSystemVisualSingles[gridPosition.x, gridPosition.y]
                .SetSpriteColor(GetGridVisualTypeColor(gridVisualType))
        );
    }
    
    private void SetGridPositionsHovered(List<GridPosition> gridPositions, bool hover)
    {
        gridPositions.ForEach(gridPosition => 
            gridSystemVisualSingles[gridPosition.x, gridPosition.y].SetHovered(hover)
        );
    }
    
    private void ResetGridPositionColor(GridPosition gridPosition)
    {
        gridSystemVisualSingles[gridPosition.x, gridPosition.y].ResetColor();
    }
    
    private void ResetAllGridPositionsColor()
    {
        foreach (GridSystemVisualSingle gridSystemVisualSingle in gridSystemVisualSingles)
            gridSystemVisualSingle.ResetColor();
    }
    
    private Color GetGridVisualTypeColor(GridVisualType gridVisualType)
    {
        return gridVisualTypeColors[gridVisualType];
    }
}