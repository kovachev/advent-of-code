using System.Diagnostics;
using System.Text.Json.Serialization;

namespace aoc_2024_16;

internal class Program
{
    private const char StartMark = 'S';
    private const char EndMark = 'E';
    private const char WallMark = '#';

    private const string PathsFile = "paths_input.json";

    private static readonly Position East = new Position(-1, 0);
    private static readonly Position North = new Position(0, -1);
    private static readonly Position South = new Position(0, 1);
    private static readonly Position West = new Position(1, 0);

    private static readonly Position[] Directions = [East, North, South, West];
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 16");
        
        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample1.txt");
        //var input = File.ReadAllLines("sample2.txt");

        var map = input.Select(x => x.ToCharArray()).ToArray();
        
        var startPosition = FindPositions(map, StartMark);
        var endPosition = FindPositions(map, EndMark);
        
        Console.WriteLine($"Looking for path from {startPosition} to {endPosition}.");

        var timestamp = Stopwatch.GetTimestamp();
        
        var path = FindPath(map, startPosition, endPosition, debug: false);

        var elapsed = Stopwatch.GetElapsedTime(timestamp);
        
        Console.WriteLine($"Path found in {elapsed:c}.");
        
        var json = System.Text.Json.JsonSerializer.Serialize(path);
        File.WriteAllText(PathsFile, json);

        Console.WriteLine($"Part 1: {path.Score}");
    }
    
    private static Position FindPositions(char[][] map, char target)
    {
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == target)
                {
                    return new Position(x, y);
                }
            }
        }

        return new Position(-1, -1);
    }
    
    private static PathWithScore? FindPath(char[][] map, Position startPosition, Position endPosition, bool debug = false)
    {
        var queue = new Stack<(Position Position, Position Direction, int Score)>();
        queue.Push((startPosition, East, 0));

        PathWithScore? result = null; 
        
        while (queue.Count > 0)
        {
            var current = queue.Pop();
            
            var currentPath = ExtractPath(current.Position).ToArray();
            
            foreach (var direction in Directions)
            {
                var neighbour = current.Position + direction;
                var newScore = current.Score + (current.Direction == direction ? 1 : 1001);
                if (result != null && result.Score < newScore)
                {
                    continue;
                }
                
                if (!neighbour.IsOnMap(map) ||
                    map[neighbour.Y][neighbour.X] == WallMark ||
                    currentPath.Any(p => p.X == neighbour.X && p.Y == neighbour.Y) ||
                    neighbour == startPosition)
                {
                    continue;
                }

                var neighbourWithParent = neighbour with { Parent = current.Position };
                
                if (neighbour == endPosition)
                {
                    var path = ExtractPath(neighbourWithParent, reverse: true).ToArray();
                    
                    if (result == null || result.Score > newScore)
                    {
                        result = new PathWithScore(path, newScore);
                    }
                    
                    continue;
                }
                
                queue.Push((neighbourWithParent, direction, newScore));

                if (debug)
                {
                    var pathWithColor = ExtractPath(neighbourWithParent).Select(p => (p, ConsoleColor.Yellow)).ToList();
                    pathWithColor[0] = (pathWithColor[0].Item1, ConsoleColor.Cyan);
                    PrintMapWithPath(map, pathWithColor);
                    Thread.Sleep(50);
                }
            }
        }
        
        return result;
    }

    private static IEnumerable<Position> ExtractPath(Position position, bool reverse = false)
    {
        var path = new List<Position>();
        
        var current = position;
        path.Add(current);
        
        while (current != null)
        {
            current = current.Parent;
            if (current != null)
            {
                path.Add(current);
            }
        }

        if (reverse)
        {
            path.Reverse();
        }
        
        return path;
    }
    
    private static void PrintMapWithPath(char[][] map, IEnumerable<(Position, ConsoleColor)>? positions = null)
    {
        Console.Clear();
        Console.ResetColor();
        
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                var position = positions.SingleOrDefault(p => p.Item1.X == x && p.Item1.Y == y);
                if (position != default)
                {
                    Console.BackgroundColor = position.Item2;
                }
                
                Console.Write(map[y][x]);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}

internal record PathWithScore(Position[] Path, int Score);

internal record Position(int X, int Y, [property: JsonIgnore] Position? Parent = null)
{
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    
    public bool IsOnMap(char[][] map) => X >= 0 && X < map[0].Length && Y >= 0 && Y < map.Length;
    
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}