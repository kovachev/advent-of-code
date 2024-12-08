namespace aoc_2024_06;

internal record Position(int X, int Y)
{
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }

    public Position Move(Direction b)
    {
        return this + b.ToPosition();
    }

    public bool IsOnMap(char[,] map)
    {
        return X >= 0 && X < map.GetLength(0) && Y >= 0 && Y < map.GetLength(1);
    }
}

internal enum Direction
{
    Up,
    Down,
    Left,
    Right
}

internal static class PositionExtensions
{
    public static Position ToPosition(this Direction direction) => direction switch
    {
        Direction.Up => new Position(0, -1),
        Direction.Down => new Position(0, 1),
        Direction.Left => new Position(-1, 0),
        Direction.Right => new Position(1, 0),
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public static Direction ToDirection(this Position position) => position switch
    {
        { X: 0, Y: -1 } => Direction.Up,
        { X: 0, Y: 1 } => Direction.Down,
        { X: -1, Y: 0 } => Direction.Left,
        { X: 1, Y: 0 } => Direction.Right,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public static Direction TurnRight(this Direction direction)
    {
        var result = direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException()
        };

        return result;
    }
    
    public static bool IsHorizontal(this Direction direction) => direction switch
    {
        Direction.Left => true,
        Direction.Right => true,
        _ => false
    };
}