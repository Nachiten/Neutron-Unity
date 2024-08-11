using System;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    private GridPosition currentGridPosition;
    private Vector3 targetPosition;
    private const float moveSpeed = 20f;
    private bool isMoving;
    
    protected virtual void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPos(transform.position);
        LevelGrid.Instance.AddGridElementAtGridPos(currentGridPosition, this);
    }

    protected virtual void Update()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        if (!isMoving)
            return; 
        
        // Calculate new position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        if (transform.position == targetPosition)
            isMoving = false;
    }
    
    protected bool Equals(GridElement other)
    {
        return base.Equals(other) && currentGridPosition.Equals(other.currentGridPosition);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GridElement)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), currentGridPosition);
    }
    
    public override string ToString()
    {
        return $"GridElement: {currentGridPosition}";
    }
}