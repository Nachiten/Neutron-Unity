using System;

[Serializable]
public readonly struct GridPosition : IEquatable<GridPosition>
{
    public readonly int x;
    public readonly int y;
    public readonly int floor;

    public GridPosition(int x, int y, int floor)
    {
        this.x = x;
        this.y = y;
        this.floor = floor;
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.y == b.y && a.floor == b.floor;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.y + b.y, a.floor + b.floor);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.y - b.y, a.floor - b.floor);
    }

    public bool FloorIsValid(int totalFloors)
    {
        return floor >= 0 && floor < totalFloors;
    }
    
    public static GridPosition Zero => new GridPosition(0, 0, 0);
    
    public static GridPosition operator *(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x * b.x, a.y * b.y, a.floor * b.floor);
    }
}