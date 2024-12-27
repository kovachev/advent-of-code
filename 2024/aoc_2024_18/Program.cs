using System.Diagnostics;
using Helpers;

namespace aoc_2024_18;

internal class Program
{
    private const char WallMarker = '#';

    private const string PathsFile = "paths_input.json";

    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 18");
        
        var input = File.ReadAllLines("input.txt");
        var map = new Map(71, 71);
        var size = 1024;
        
        // var input = File.ReadAllLines("sample.txt");
        // var map = new Map(7, 7);
        // var size = 12;

        foreach (var line in input.Take(size))
        {
            var parts = line.Split(',');
            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);
            map[x, y] = WallMarker;
        }
        
        var startPosition = new Position(0, 0);
        var endPosition = new Position(map.XMax - 1, map.YMax - 1);
        
        Console.WriteLine($"Looking for path from {startPosition} [{map[startPosition]}] to {endPosition} [{map[endPosition]}].");

        //map.Print();
        //return;
        
        var timestamp = Stopwatch.GetTimestamp();
        
        var path = FindPath(map, startPosition, endPosition, debug: false);

        var elapsed = Stopwatch.GetElapsedTime(timestamp);
        
        Console.WriteLine($"Path found in {elapsed:c}.");
        
        var json = System.Text.Json.JsonSerializer.Serialize(path);
        File.WriteAllText(PathsFile, json);

        //PrintMapWithPath(map, path?.Path.Select(p => (p, ConsoleColor.Yellow)));

        Console.WriteLine($"Part 1: {path?.Score}");
        
        //Console.ReadLine();
    }
    
    // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    private static PathWithScore? FindPath(Map map, Position startPosition, Position endPosition, bool debug = false)
    {
        if (debug)
        {
            Console.Clear();
            map.Print();
        }
        
        var queue = new PriorityQueue<Position, int>();
        queue.Enqueue(startPosition, 0);

        PathWithScore? result = null; 

        var visited = new HashSet<Position>();
        foreach (var position in map.Where(x => x.Value == WallMarker).Select(x => x.Position))
        {
            visited.Add(position);
        }

        while (queue.Count > 0)
        {
            if (!queue.TryDequeue(out var current, out var score))
            {
                return result;
            }
            
            visited.Add(current);
            
            foreach (var neighbour in map.GetNeighbours(current))
            {
                var newScore = score + 1;
                if (result != null && result.Score < newScore)
                {
                    continue;
                }
                
                if (visited.Any(p => p.X == neighbour.X && p.Y == neighbour.Y))
                {
                    continue;
                }
                
                var neighbourWithParent = neighbour with { Parent = current };
                
                if (neighbour == endPosition)
                {
                    var path = ExtractPath(neighbourWithParent, reverse: true).ToArray();
                    
                    if (result == null || result.Score > newScore)
                    {
                        result = new PathWithScore(path, newScore);
                    }
                    
                    Console.WriteLine($"Path found with score {newScore}.");
                    continue;
                }
                
                visited.Add(neighbour);
                queue.Enqueue(neighbourWithParent, newScore);

                if (debug)
                {
                    var pathWithColor = ExtractPath(neighbourWithParent).Select(p => (p, ConsoleColor.Yellow)).ToList();
                    pathWithColor[0] = (pathWithColor[0].Item1, ConsoleColor.Cyan);
                    //PrintMapWithPath(map, pathWithColor);
                    ShowPath(map, pathWithColor);
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

    private static IEnumerable<Position>? _prevPositions = null;

    private static void ShowPath(Map map, IEnumerable<(Position, ConsoleColor)> positions)
    {
        if (_prevPositions?.Any() == true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            foreach (var position in _prevPositions)
            {
                Console.SetCursorPosition(position.X, position.Y);
                Console.Write(map[position]);
            }
        }

        foreach (var (position, color) in positions)
        {
            Console.SetCursorPosition(position.X, position.Y);
            Console.BackgroundColor = color;
            Console.Write(map[position]);
        }

        _prevPositions = positions.Select(x => x.Item1);
    }
    
    private static void PrintMapWithPath(Map map, IEnumerable<(Position, ConsoleColor)>? positions = null)
    {
        Console.Clear();
        
        for (var y = 0; y < map.YMax; y++)
        {
            for (var x = 0; x < map.XMax; x++)
            {
                var position = positions?.SingleOrDefault(p => p.Item1.X == x && p.Item1.Y == y);
                if (position is not null)
                {
                    Console.BackgroundColor = position.Value.Item2;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                
                Console.Write(map[x , y]);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}

internal record PathWithScore(Position[] Path, int Score);