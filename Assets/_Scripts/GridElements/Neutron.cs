using System.Collections.Generic;

public class Neutron : GridElement
{
    public override List<GridPosition> GetAvailableMovePositions()
    {
        List<GridPosition> availableMovePositions = new()
        {
            new GridPosition(currentGridPosition.x - 1, currentGridPosition.y - 1),
            new GridPosition(currentGridPosition.x, currentGridPosition.y - 1),
            new GridPosition(currentGridPosition.x + 1, currentGridPosition.y - 1),
            new GridPosition(currentGridPosition.x - 1, currentGridPosition.y),
            new GridPosition(currentGridPosition.x + 1, currentGridPosition.y),
            new GridPosition(currentGridPosition.x - 1, currentGridPosition.y + 1),
            new GridPosition(currentGridPosition.x, currentGridPosition.y + 1),
            new GridPosition(currentGridPosition.x + 1, currentGridPosition.y + 1)
        };
        
        availableMovePositions.RemoveAll(pos => !LevelGrid.Instance.GridPosIsValid(pos));
        
        return availableMovePositions;
    }
}
