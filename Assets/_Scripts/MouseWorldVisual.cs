using UnityEngine;

public class MouseWorldVisual : MonoBehaviour
{
    [SerializeField] private LayerMask mousePlaneLayerMask;
    
    private Camera mainCamera;
    
    // PRIVATE Singleton
    private static MouseWorldVisual Instance;
    
    private void Awake()
    { 
        Instance = this;      
        
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        transform.position = GetMouseWorldPosition();
    }
    
    private static Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPosition = InputManager.Instance.GetMouseScreenPosition();
        
        Ray ray = Instance.mainCamera.ScreenPointToRay(mouseScreenPosition);
        
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, 15f, Instance.mousePlaneLayerMask);
        
        System.Array.Sort(raycastHits, (x, y) => x.distance.CompareTo(y.distance));
        
        foreach (RaycastHit raycastHit in raycastHits)
        {
            // If there is a renderer, and it is enabled, return the point
            if (raycastHit.transform.TryGetComponent(out Renderer renderer) && renderer.enabled)
                return raycastHit.point;
        }
        
        return Vector3.zero;
    }
    
    public static GridPosition GetMouseGridPosition()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        
        return LevelGrid.Instance.GetGridPos(mouseWorldPosition);
    }
}