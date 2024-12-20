using Helpers;

namespace aoc_2024_15;

internal class Program
{
    private const char RobotMarker = '@';
    private const char WallMarker = '#';
    private const char EmptyMarker = '.';
    private const char BoxMarker = 'O';
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 15");
        
        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample1.txt");
        //var input = File.ReadAllLines("sample2.txt");
        
        var (map, moves) = ParseInput(input);
        
        Part1(map, moves);
    }
    
    private static void Part1(Map map, Position[] moves)
    {
        var robotPosition = map.Single(x => x.Value == RobotMarker).Position;
        
        foreach (var move in moves)
        {
            var newPosition = robotPosition.Move(move);
            
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
                map[newPosition] = RobotMarker;
                map[boxPosition] = BoxMarker;
                robotPosition = newPosition;
            }
            else
            {
                map[robotPosition] = EmptyMarker;
                map[newPosition] = RobotMarker;
                robotPosition = newPosition;
            }
            
            // Console.Clear();
            // map.Print();
            // Thread.Sleep(250);
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
    
    private static (Map, Position[]) ParseInput(string[] lines)
    {
        var map = new Map(lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x))
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
}