namespace Helpers;

public static class PositionExtensions
{
    // public static Position ToPosition(this Direction direction) => direction switch
    // {
    //     Direction.Up => Position.Up,
    //     Direction.Down => Position.Down,
    //     Direction.Left => Position.Left,
    //     Direction.Right => Position.Right,
    //     _ => throw new ArgumentOutOfRangeException()
    // };
    //
    // public static Direction ToDirection(this Position position) => position switch
    // {
    //     { X: 0, Y: -1 } => Direction.Up,
    //     { X: 0, Y: 1 } => Direction.Down,
    //     { X: -1, Y: 0 } => Direction.Left,
    //     { X: 1, Y: 0 } => Direction.Right,
    //     _ => throw new ArgumentOutOfRangeException()
    // };
    //
    // public static Direction TurnRight(this Direction direction)
    // {
    //     var result = direction switch
    //     {
    //         Direction.Up => Direction.Right,
    //         Direction.Right => Direction.Down,
    //         Direction.Down => Direction.Left,
    //         Direction.Left => Direction.Up,
    //         _ => throw new ArgumentOutOfRangeException()
    //     };
    //
    //     return result;
    // }
    //
    // public static bool IsHorizontal(this Direction direction) => direction switch
    // {
    //     Direction.Left => true,
    //     Direction.Right => true,
    //     _ => false
    // };
}