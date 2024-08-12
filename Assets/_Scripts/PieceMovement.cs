using System;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    [SerializeField] private GridPositionSelection gridPositionSelection;

    public Action<GridPosition> OnPieceSelected;

    private GridPosition selectedPiece = GridPosition.Null;
    
    private enum State
    {
        SelectingPiece,
        SelectingMove,
        MovingPiece
    }
    
    private State state;
    
    private void Start()
    {
        gridPositionSelection.OnGridPositionSelected += OnGridPositionSelected;
        gridPositionSelection.OnGridPositionUnselected += OnGridPositionUnselected;
        
        state = State.SelectingPiece;
    }

    private void OnGridPositionSelected(GridPosition gridPosition)
    {
        switch (state)
        {
            case State.SelectingPiece:
                // Check if there is a piece at the selected position
                // If there is a piece, emit event and change state to SelectingMove

                if (!LevelGrid.Instance.GridPosHasAnyGridElement(gridPosition))
                    return;
                
                OnPieceSelected?.Invoke(gridPosition);
                
                selectedPiece = gridPosition;
                state = State.SelectingMove;
                break;
            case State.SelectingMove:
                // Select move
                // Check if move is valid
                // If valid, move the piece and change state to MovingPiece
                // If not valid, do nothing
                break;
            case State.MovingPiece:
                // Do nothing, wait for event of piece finished moving
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnGridPositionUnselected(GridPosition gridPosition)
    {
        switch (state)
        {
            case State.SelectingPiece:
                break;
            case State.SelectingMove:
                if (gridPosition == selectedPiece)
                {
                    state = State.SelectingPiece;
                    selectedPiece = GridPosition.Null;
                }
                break;
            case State.MovingPiece:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // private void Update()
    // {
    //     switch (state)
    //     {
    //         case State.SelectingPiece:
    //             HandleSelectingPiece();
    //             break;
    //         case State.SelectingMove:
    //             HandleSelectingMove();
    //             break;
    //         case State.MovingPiece:
    //             HandleMovingPiece();
    //             break;
    //     }
    // }
    //
    // private void HandleMovingPiece()
    // {
    //     throw new System.NotImplementedException();
    // }
    //
    // private void HandleSelectingMove()
    // {
    //     throw new System.NotImplementedException();
    // }
    //
    // private void HandleSelectingPiece()
    // {
    //     throw new System.NotImplementedException();
    // }
}
