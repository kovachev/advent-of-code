using System.Diagnostics;
using Helpers;

namespace aoc_2024_16;

internal class Program
{
    private const char StartMarker = 'S';
    private const char EndMarker = 'E';
    private const char WallMarker = '#';

    private const string PathsFile = "paths_input.json";

    private static readonly Position[] Directions = [Position.East, Position.West, Position.South, Position.North];

    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 16");
        
        var map = new Map("input.txt");
        //var map = new Map("sample1.txt");
        //var map = new Map("sample2.txt");

        var startPosition = map.Single(x => x.Value == StartMarker).Position;
        var endPosition = map.Single(x => x.Value == EndMarker).Position;
        
        Console.WriteLine($"Looking for path from {startPosition} to {endPosition}.");

        var timestamp = Stopwatch.GetTimestamp();
        
        var path = FindPath(map, startPosition, endPosition, debug: false);

        var elapsed = Stopwatch.GetElapsedTime(timestamp);
        
        Console.WriteLine($"Path found in {elapsed:c}.");
        
        var json = System.Text.Json.JsonSerializer.Serialize(path);
        File.WriteAllText(PathsFile, json);

        Console.WriteLine($"Part 1: {path?.Score}");

        //Console.ReadLine();
    }
    
    // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    private static PathWithScore? FindPath(Map map, Position startPosition, Position endPosition, bool debug = false)
    {
        var queue = new PriorityQueue<(Position Position, Position Direction), int>();
        queue.Enqueue((startPosition, Position.East), 0);

        PathWithScore? result = null; 
        var visited = new HashSet<Position>();
        
        while (queue.Count > 0)
        {
            if (!queue.TryDequeue(out var current, out var score))
            {
                return result;
            }
            
            visited.Add(current.Position);
            
            //var currentPath = ExtractPath(current.Position).ToArray();
            
            foreach (var direction in Directions)
            {
                var neighbour = current.Position + direction;
                var newScore = score + (current.Direction == direction ? 1 : 1001);
                if (result != null && result.Score < newScore)
                {
                    continue;
                }
                
                if (!map.IsOnMap(neighbour) ||
                    map[neighbour] == WallMarker ||
                    visited.Any(p => p.X == neighbour.X && p.Y == neighbour.Y) ||
                    //currentPath.Any(p => p.X == neighbour.X && p.Y == neighbour.Y) ||
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
                
                queue.Enqueue((neighbourWithParent, direction), newScore);

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
    
    private static void PrintMapWithPath(Map map, IEnumerable<(Position, ConsoleColor)>? positions = null)
    {
        Console.Clear();
        Console.ResetColor();
        
        for (var y = 0; y < map.YMax; y++)
        {
            for (var x = 0; x < map.XMax; x++)
            {
                var position = positions?.SingleOrDefault(p => p.Item1.X == x && p.Item1.Y == y);
                if (position is not null)
                {
                    Console.BackgroundColor = position.Value.Item2;
                }
                
                Console.Write(map[x , y]);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}

internal record PathWithScore(Position[] Path, int Score);