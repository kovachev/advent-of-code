using Helpers;

namespace aoc_2024_18;

internal class Program
{
    private const char WallMarker = '#';

    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 18");
        
        Part1("sample.txt", new Map(7, 7), 12);
        
        Part1("input.txt", new Map(71, 71), 1024);
        
        Part2("sample.txt", new Map(7, 7), 12);
        
        Part2("input.txt", new Map(71, 71), 1024);
    }

    private static void Part1(string inputFile, Map map, int takeSize)
    {
        var startPosition = new Position(0, 0);
        var endPosition = new Position(map.XMax - 1, map.YMax - 1);
        
        var input = File.ReadAllLines(inputFile);
        AddWallsToMap(input, map, takeSize);
        
        Console.WriteLine($"Looking for path from {startPosition} [{map[startPosition]}] to {endPosition} [{map[endPosition]}].");
        
        var path = FindPath(map, startPosition, endPosition, debug: false);

        Console.WriteLine($"Part 1 {inputFile}: {path?.Score}");
    }
    
    private static void Part2(string inputFile, Map map, int takeSize)
    {
        var startPosition = new Position(0, 0);
        var endPosition = new Position(map.XMax - 1, map.YMax - 1);
        
        var input = File.ReadAllLines(inputFile);
        
        var (lo, hi) = (takeSize, input.Length);
        while (hi - lo > 1) {
            var m = (lo + hi) / 2;
            AddWallsToMap(input, map, m);
            if (FindPath(map, startPosition, endPosition) == null) {
                hi = m;
            } else {
                lo = m;
            }
        }
        
        Console.WriteLine($"Part 2 {inputFile}: {input[lo]}");
    }
    
    private static void AddWallsToMap(string[] lines, Map map, int takeSize)
    {
        map.Clear();
        foreach (var line in lines.Take(takeSize))
        {
            var parts = line.Split(',');
            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);
            map[x, y] = WallMarker;
        }
    }
    
    // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    private static PathAndScore? FindPath(Map map, Position startPosition, Position endPosition, bool debug = false)
    {
        if (debug)
        {
            Console.Clear();
            map.Print();
        }
        
        var queue = new PriorityQueue<Position, int>();
        queue.Enqueue(startPosition, 0);

        PathAndScore? result = null; 

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
                    var path = neighbourWithParent.ExtractPath(reverse: true).ToArray();
                    
                    if (result == null || result.Score > newScore)
                    {
                        result = new PathAndScore(path, newScore);
                    }
                    
                    continue;
                }
                
                visited.Add(neighbour);
                queue.Enqueue(neighbourWithParent, newScore);

                if (debug)
                {
                    var pathWithColor = neighbourWithParent.ExtractPath().Select(p => (p, ConsoleColor.Yellow)).ToList();
                    pathWithColor[0] = (pathWithColor[0].Item1, ConsoleColor.Cyan);
                    //PrintMapWithPath(map, pathWithColor);
                    ShowPath(map, pathWithColor);
                    Thread.Sleep(50);
                }
            }
        }
        
        return result;
    }

    private static IEnumerable<Position>? _prevPositions;

    private static void ShowPath(Map map, IEnumerable<(Position, ConsoleColor)> positions)
    {
        if (_prevPositions?.Any() == true)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
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