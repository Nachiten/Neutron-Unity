using System;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    [SerializeField] private GridPositionSelection gridPositionSelection;

    public Action<GridPosition> OnPieceSelected;
    public Action<GridPosition> OnPieceUnselected;
    public Action<GridPosition> OnMoveStarted;
    
    private GridPosition selectedPiece = GridPosition.Null;
    private GridElement selectedGridElement;
    
    private enum State
    {
        // Description: The player is selecting a piece to move
        SelectingPiece,
        // Description: The player is selecting a move position for the selected piece
        SelectingMove,
        // Description: The selected piece is moving to the selected move position
        MovingPiece
    }
    
    private State state;
    
    private void Start()
    {
        gridPositionSelection.OnGridPositionSelected += OnGridPositionSelected;
        LevelGrid.Instance.OnAnyGridElementMovedGridPosition += OnAnyGridElementMovedGridPosition;
        
        state = State.SelectingPiece;
    }

    private void OnAnyGridElementMovedGridPosition(GridElement gridElement, GridPosition fromGridPos, GridPosition toGridPos)
    {
        if (state != State.MovingPiece || gridElement != selectedGridElement)
            return;
        
        state = State.SelectingPiece;
        selectedPiece = GridPosition.Null;
    }

    private void OnGridPositionSelected(GridPosition gridPosition)
    {
        switch (state)
        {
            case State.SelectingPiece:
                if (!LevelGrid.Instance.GridPosHasAnyGridElement(gridPosition))
                    return;
                
                selectedPiece = gridPosition;
                selectedGridElement = LevelGrid.Instance.GetGridElementAtGridPos(gridPosition);
                
                OnPieceSelected?.Invoke(gridPosition);
                
                state = State.SelectingMove;
                break;
            case State.SelectingMove:
                if (gridPosition == selectedPiece)
                {
                    state = State.SelectingPiece;
                    selectedPiece = GridPosition.Null;
                    selectedGridElement = null;
                    OnPieceUnselected?.Invoke(gridPosition);
                    return;
                }
                
                if (LevelGrid.Instance.GridPosHasAnyGridElement(gridPosition))
                { 
                    state = State.SelectingPiece;
                    OnGridPositionSelected(gridPosition);
                }
                
                if (!selectedGridElement.IsMovePositionValid(gridPosition))
                    return;
                
                selectedGridElement.MoveToGridPosition(gridPosition);
                OnMoveStarted?.Invoke(gridPosition);
                state = State.MovingPiece;
                break;
            case State.MovingPiece:
                // Do nothing, wait for event of piece finished moving
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
