using System.Text.Json.Serialization;

namespace Helpers;

public record Position(int X, int Y, [property: JsonIgnore] Position? Parent = null)
{
    public static Position Up => new(0, -1);
    public static Position Down => new(0, 1);
    public static Position Left => new(-1, 0);
    public static Position Right => new(1, 0);
    
    public static Position North => Up;
    public static Position South => Down;
    public static Position West => Left;
    public static Position East => Right;
    
    public static Position UpLeft => new(-1, -1);
    public static Position UpRight => new(1, -1);
    public static Position DownLeft => new(-1, 1);
    public static Position DownRight => new(1, 1);
    
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);

    public Position Move(Position difference)
    {
        return this + difference;
    }
    
    public virtual bool Equals(Position? other)
    {
        if (other is null)
        {
            return false;
        }
        
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public static bool AreOnSameRow(params Position[] positions)
    {
        var first = positions.First();
        return positions.All(x => x.X == first.X);
    }
    
    public static bool AreOnSameColumn(params Position[] positions)
    {
        var first = positions.First();
        return positions.All(x => x.Y == first.Y);
    }

    public static bool AreOnSameDiagonal(params Position[] positions)
    {
        var first = positions.First();
        return positions.All(x => x.Y - x.X == first.Y - first.X) || // Same diagonal (top-left to bottom-right)
               positions.All(x => x.Y + x.X == first.Y + first.X); // Same diagonal (top-right to bottom-left)
    }
    
    public Position TurnRight()
    {
        if (this == Up)
        {
            return Right;
        }

        if (this == Down)
        {
            return Left;
        }

        if (this == Left)
        {
            return Up;
        }

        if (this == Right)
        {
            return Down;
        }

        throw new ArgumentOutOfRangeException();
    }
    
    public bool IsHorizontal()
    {
        return this == Left || this == Right;
    }
    
    public bool IsVertical()
    {
        return this == Up || this == Down;
    }
}