using System.Text;
using Helpers;

namespace aoc_2024_15;

internal class Program
{
    private const char RobotMarker = '@';
    private const char WallMarker = '#';
    private const char EmptyMarker = '.';
    private const char BoxMarker = 'O';
    private const char BigBoxMarkerLeft = '[';
    private const char BigBoxMarkerRight = ']';

    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 15");

        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample1.txt");
        //var input = File.ReadAllLines("sample2.txt");

        var (map, moves) = ParseInput(input, s => s);

        var debug = false;

        Console.WriteLine($"Total moves: {moves.Length}");

        Part1(map, moves, debug);

        debug = true;
        
        var (wideMap, _) = ParseInput(input, Expand);

        Part2(wideMap, moves, debug);
    }

    private static void Part1(Map map, Position[] moves, bool debug)
    {
        var robotPosition = map.Single(x => x.Value == RobotMarker).Position;

        if (debug)
        {
            Console.Clear();
            map.Print();
        }

        var totalMoves = moves.Length;
        var movesCount = 0;
        foreach (var move in moves)
        {
            movesCount++;

            var newPosition = robotPosition + move;
            
            if (map[newPosition] == WallMarker)
            {
                continue;
            }

            if (map[newPosition] == BoxMarker)
            {
                var boxPosition = newPosition + move;

                while (map[boxPosition] != WallMarker &&
                       map[boxPosition] != EmptyMarker)
                {
                    boxPosition += move;
                }

                if (map[boxPosition] == WallMarker)
                {
                    continue;
                }

                map[robotPosition] = EmptyMarker;
                ConsoleReplace(robotPosition, EmptyMarker, debug);
                map[newPosition] = RobotMarker;
                ConsoleReplace(newPosition, RobotMarker, debug);
                map[boxPosition] = BoxMarker;
                ConsoleReplace(boxPosition, BoxMarker, debug);

                robotPosition = newPosition;
            }
            else
            {
                map[robotPosition] = EmptyMarker;
                ConsoleReplace(robotPosition, EmptyMarker, debug);
                map[newPosition] = RobotMarker;
                ConsoleReplace(newPosition, RobotMarker, debug);

                robotPosition = newPosition;
            }

            if (debug)
            {
                Console.SetCursorPosition(map.XMax + 1, 0);
                Console.WriteLine($"Move {movesCount} of {totalMoves}");
                Thread.Sleep(100);
            }
        }

        var result = 0;

        foreach (var (position, value) in map)
        {
            if (value == BoxMarker)
            {
                result += position.X + position.Y * 100;
            }
        }

        Console.WriteLine($"Part 1: {result}");
    }

    private static void Part2(Map map, Position[] moves, bool debug)
    {
        var robotPosition = map.Single(x => x.Value == RobotMarker).Position;

        if (debug)
        {
            Console.Clear();
            map.Print();
        }

        var totalMoves = moves.Length;
        var movesCount = 0;
        foreach (var move in moves)
        {
            movesCount++;

            var newPosition = robotPosition + move;

            if (map[newPosition] == WallMarker)
            {
                continue;
            }

            if (map[newPosition] == BigBoxMarkerLeft ||
                map[newPosition] == BigBoxMarkerRight)
            {
                if (move.IsVertical())
                {
                    if (!TryMoveBigBoxVertically(map, newPosition, move, debug))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!TryMoveBigBoxHorizontally(map, newPosition, move, debug))
                    {
                        continue;
                    }
                }

                map[robotPosition] = EmptyMarker;
                ConsoleReplace(robotPosition, EmptyMarker, debug);
                map[newPosition] = RobotMarker;
                ConsoleReplace(newPosition, RobotMarker, debug);
                
                robotPosition = newPosition;
            }
            else
            {
                map[robotPosition] = EmptyMarker;
                ConsoleReplace(robotPosition, EmptyMarker, debug);
                map[newPosition] = RobotMarker;
                ConsoleReplace(newPosition, RobotMarker, debug);
                
                robotPosition = newPosition;
            }

            if (debug)
            {
                Console.SetCursorPosition(map.XMax + 1, 0);
                Console.WriteLine($"Move {movesCount} of {totalMoves}");
                Thread.Sleep(10);
            }
        }

        var result = 0;

        foreach (var (position, value) in map)
        {
            if (value == BigBoxMarkerLeft)
            {
                result += position.X + position.Y * 100;
            }
        }

        map.Print();

        Console.WriteLine($"Part 2: {result}");
    }

    private static bool TryMoveBigBoxHorizontally(Map map, Position boxPosition, Position direction, bool debug)
    {
        if (map[boxPosition] == BigBoxMarkerRight)
        {
            boxPosition += Position.Left;
        }

        var newPositionLeft = boxPosition + direction;
        var newPositionRight = newPositionLeft + Position.Right;

        if (map[newPositionLeft] == WallMarker ||
            map[newPositionRight] == WallMarker)
        {
            return false;
        }

        if (direction == Position.Left)
        {
            if (map[newPositionLeft] == BigBoxMarkerRight)
            {
                if (!TryMoveBigBoxHorizontally(map, newPositionLeft, direction, debug))
                {
                    return false;
                }
            }
        }

        if (direction == Position.Right)
        {
            if (map[newPositionRight] == BigBoxMarkerLeft)
            {
                if (!TryMoveBigBoxHorizontally(map, newPositionRight, direction, debug))
                {
                    return false;
                }
            }
        }

        map[boxPosition] = EmptyMarker;
        ConsoleReplace(boxPosition, EmptyMarker, debug);
        
        map[boxPosition + Position.Right] = EmptyMarker;
        ConsoleReplace(boxPosition + Position.Right, EmptyMarker, debug);
        
        map[newPositionLeft] = BigBoxMarkerLeft;
        ConsoleReplace(newPositionLeft, BigBoxMarkerLeft, debug);
        
        map[newPositionRight] = BigBoxMarkerRight;
        ConsoleReplace(newPositionRight, BigBoxMarkerRight, debug);

        return true;
    }

    private static bool TryMoveBigBoxVertically(Map map, Position boxPosition, Position direction, bool debug)
    {
        if (map[boxPosition] == BigBoxMarkerRight)
        {
            boxPosition += Position.Left;
        }

        var newPositionLeft = boxPosition + direction;
        var newPositionRight = newPositionLeft + Position.Right;

        if (map[newPositionLeft] == WallMarker ||
            map[newPositionRight] == WallMarker)
        {
            return false;
        }

        var mapCopy = map.Clone();
        
        if (map[newPositionLeft] == BigBoxMarkerLeft ||
            map[newPositionLeft] == BigBoxMarkerRight)
        {
            if (!TryMoveBigBoxVertically(map, newPositionLeft, direction, debug))
            {
                return false;
            }
        }

        if (map[newPositionRight] == BigBoxMarkerLeft ||
            map[newPositionRight] == BigBoxMarkerRight)
        {
            if (!TryMoveBigBoxVertically(map, newPositionRight, direction, debug))
            {
                map.Restore(mapCopy);
                return false;
            }
        }

        map[boxPosition] = EmptyMarker;
        ConsoleReplace(boxPosition, EmptyMarker, debug);
        
        map[boxPosition + Position.Right] = EmptyMarker;
        ConsoleReplace(boxPosition + Position.Right, EmptyMarker, debug);
        
        map[newPositionLeft] = BigBoxMarkerLeft;
        ConsoleReplace(newPositionLeft, BigBoxMarkerLeft, debug);
        
        map[newPositionRight] = BigBoxMarkerRight;
        ConsoleReplace(newPositionRight, BigBoxMarkerRight, debug);

        return true;
    }

    private static (Map, Position[]) ParseInput(string[] lines, Func<string, string> transform)
    {
        var map = new Map(lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                               .Select(transform)
                               .ToArray());

        var movesChars = string.Join("", lines.SkipWhile(x => !string.IsNullOrWhiteSpace(x))
                                              .Skip(1)
                                              .Select(x => x.Trim()))
                               .ToCharArray();

        var moves = movesChars.Select(x => x switch
        {
            '^' => Position.Up,
            '>' => Position.Right,
            'v' => Position.Down,
            '<' => Position.Left,
            _ => throw new ArgumentOutOfRangeException()
        }).ToArray();

        return (map, moves);
    }

    private static string Expand(string line)
    {
        var sb = new StringBuilder();

        foreach (var c in line)
        {
            switch (c)
            {
                case '#':
                    sb.Append("##");
                    break;

                case 'O':
                    sb.Append("[]");
                    break;

                case '.':
                    sb.Append("..");
                    break;

                case '@':
                    sb.Append("@.");
                    break;
            }
        }

        return sb.ToString();
    }

    private static void ConsoleReplace(Position position, char value, bool debug)
    {
        if (!debug)
        {
            return;
        }

        Console.SetCursorPosition(position.X, position.Y);
        Console.Write(value);
    }
}