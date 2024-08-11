using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private Transform gridSystemVisualSingleParent;
    [SerializeField] private LayerMask objectivesLayerMask;

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    private void Start()
    {
        int width = LevelGrid.Instance.GetWidth();
        int height = LevelGrid.Instance.GetHeight();

        gridSystemVisualSingles = new GridSystemVisualSingle[width, height];

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 worldPos = LevelGrid.Instance.GetWorldPos(new GridPosition(x, y));
            const float raycastOffsetDistance = 1f;
            
            // Check if there is already an objective on this grid position
            bool objectivesRaycast = Physics.Raycast(
                worldPos + Vector3.up * raycastOffsetDistance,
                Vector3.down,
                raycastOffsetDistance * 2,
                objectivesLayerMask);

            if (objectivesRaycast) 
                continue;
            
            Transform gridSystemVisual =
                Instantiate(gridSystemVisualSinglePrefab, 
                    worldPos, 
                    Quaternion.identity, 
                    gridSystemVisualSingleParent);
                
            gridSystemVisualSingles[x, y] = gridSystemVisual.GetComponent<GridSystemVisualSingle>();
        }
    }
}