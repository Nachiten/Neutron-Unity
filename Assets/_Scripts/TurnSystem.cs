using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    [SerializeField] private PieceMovement pieceMovement;

    public Action<int> OnTurnChanged;
    
    private int currentPlayerIndex;
    private int currentTurn;

    private enum State
    { 
        // Description: The player is moving the Neutron
        MovingNeutron,
        // Description: The player is moving an Electron
        MovingElectron,
        // Description: The script is waiting for the movement to finish
        WaitingForMovementFinish
    }
    
    private State state;
    private State nextState;

    private void Awake()
    {
        currentPlayerIndex = 0;
        state = State.MovingNeutron;
    }

    private void Start()
    {
        pieceMovement.OnMoveStarted += OnMoveStarted;
        pieceMovement.OnMoveEnded += OnMoveEnded;
    }

    private void OnMoveStarted(GridPosition obj)
    {
        switch (state)
        {
            case State.MovingNeutron:
                state = State.WaitingForMovementFinish;
                nextState = State.MovingElectron;
                break;
            case State.MovingElectron:
                state = State.WaitingForMovementFinish;
                nextState = State.MovingNeutron;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnMoveEnded(GridPosition obj)
    {
        state = nextState;
        
        if (state == State.MovingNeutron)
        {
            currentTurn++;
            currentPlayerIndex = currentTurn % 2;
            
            OnTurnChanged?.Invoke(currentPlayerIndex);
        }
    }

    public bool IsValidGridPosForTurn(GridPosition gridPosition)
    {
        GridElement gridElement = LevelGrid.Instance.GetGridElementAtGridPos(gridPosition);
        
        return state switch
        {
            State.MovingNeutron => gridElement is Neutron,
            State.MovingElectron => gridElement.TryGetComponent(out Electron electron) &&
                                    electron.GetPlayerIndex() == currentPlayerIndex,
            _ => false
        };
    }
}
