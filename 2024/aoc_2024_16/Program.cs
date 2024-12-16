using System.Text.Json.Serialization;

namespace aoc_2024_16;

internal class Program
{
    private const char StartMark = 'S';
    private const char EndMark = 'E';
    private const char WallMark = '#';

    private const string PathsFile = "paths_input.json";
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 16");
        
        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample1.txt");
        //var input = File.ReadAllLines("sample2.txt");

        var map = input.Select(x => x.ToCharArray()).ToArray();
        
        var startPosition = FindPositions(map, StartMark);
        var endPosition = FindPositions(map, EndMark);
        
        Console.WriteLine($"Looking for path from {startPosition} to {endPosition}");
        
        // if (File.Exists(PathsFile))
        // {
        //     var pathz = System.Text.Json.JsonSerializer.Deserialize<Position[][]>(File.ReadAllText(PathsFile));
        //     
        //     Part1(map, pathz);            
        //     
        //     return;
        // }
        
        var paths = FindPaths(map, startPosition, endPosition, debug: false);

        var json = System.Text.Json.JsonSerializer.Serialize(paths);
        File.WriteAllText(PathsFile, json);

        Part1(map, paths);
    }

    private static void Part1(char[][] map, IEnumerable<IEnumerable<Position>> paths)
    {
        var scores = new List<long>();
        foreach (var path in paths)
        {
            var score = ComputePathScore(path.ToArray(), out var corners);
            
            scores.Add(score);

            // var pathWithColors = path.Select(x => (x, corners.Any(c => c.X == x.X && c.Y == x.Y) ? ConsoleColor.Red : ConsoleColor.Yellow));
            //
            // PrintMapWithPath(map, pathWithColors);
            // Console.WriteLine($"Score: {score} | Length: {path.Count()}");
            // Console.ReadLine();
        }
        
        Console.WriteLine($"Part 1: {scores.Min()}");
    }
    
    private static long ComputePathScore(Position[] path, out ICollection<Position> corners)
    {
        corners = new List<Position>();
        
        var score = path.Length - 1;
        
        // Initially pointing east
        if (path.Length > 2)
        {
            var diff = path[1] - path[0];

            if (diff.X is 0)
            {
                score += 1000;
                corners.Add(path[0]);
            }
            else if (diff.X == -1)
            {
                score += 2000;
                corners.Add(path[0]);
            }
        }
        
        if (path.Length < 3)
        {
            return score;
        }
        
        for (var i = 1; i < path.Length - 1; i++)
        {
            var prev = path[i - 1];
            var next = path[i + 1];
            
            var diff = next - prev;
            
            if (diff.X != 0 && diff.Y != 0)
            {
                score += 1000;
                corners.Add(path[i]);
            }
            
        }

        return score;
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
    
    private static IEnumerable<Position> GetNeighbours(Position position)
    {
        yield return position + new Position(0, 1);
        yield return position + new Position(1, 0);
        yield return position + new Position(0, -1);
        yield return position + new Position(-1, 0);
    }

    private static IEnumerable<IEnumerable<Position>> FindPaths(char[][] map, Position startPosition, Position endPosition, bool debug = false)
    {
        var queue = new Stack<Position>();
        queue.Push(startPosition);
            
        var paths = new List<Position[]>();

        var i = 0;
        
        var minScore = long.MaxValue;
        
        while (queue.Count > 0)
        {
            if (debug)
            {
                PrintMap(map);
            }

            i++;
            if (i % 250000 == 0)
            {
                Console.WriteLine($"{i,10}: {queue.Count} | Paths: {paths.Count} | Score: {minScore}");
                i = 0;
            }
            
            var current = queue.Pop();

            var currentPath = ExtractPath(current).ToArray();
            
            var currentScore = ComputePathScore(currentPath, out _);
            if (currentScore > minScore)
            {
                continue;
            }
            
            foreach (var neighbour in GetNeighbours(current))
            {
                if (!neighbour.IsOnMap(map) ||
                    map[neighbour.Y][neighbour.X] == WallMark ||
                    //tested.Any(p => p.X == neighbour.X && p.Y == neighbour.Y) ||
                    currentPath.Any(p => p.X == neighbour.X && p.Y == neighbour.Y) ||
                    neighbour == startPosition)
                {
                    continue;
                }

                var neighbourWithParent = neighbour with { Parent = current };
                
                
                
                if (neighbour == endPosition)
                {
                    var path = ExtractPath(neighbourWithParent, reverse: true).ToArray();
                    paths.Add(path);
                    
                    currentScore = ComputePathScore(path, out _);
                    if (minScore > currentScore)
                    {
                        minScore = currentScore;
                    }
                    
                    continue;
                }

                queue.Push(neighbourWithParent);

                if (debug)
                {
                    var pathWithColor = ExtractPath(neighbourWithParent).Select(p => (p, ConsoleColor.Yellow)).ToList();
                    pathWithColor[0] = (pathWithColor[0].Item1, ConsoleColor.Cyan);
                    OverlayPositions(map, pathWithColor);
                    Thread.Sleep(50);
                }
            }
        }
        
        return paths;
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
    
    private static void PrintMap(char[][] map)
    {
        Console.Clear();
        Console.ResetColor();
        
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                Console.Write(map[y][x]);
            }

            Console.WriteLine();
        }
    }

    private static void OverlayPositions(char[][] map, IEnumerable<(Position, ConsoleColor)>? positions = null)
    {
        var left = Console.CursorLeft;
        var top = Console.CursorTop;
        
        foreach (var (position, color) in positions)
        {
            Console.CursorLeft = position.X;
            Console.CursorTop = position.Y;

            Console.BackgroundColor = color;
            Console.Write(map[position.Y][position.X]);
            Console.ResetColor();
        }

        Console.CursorLeft = left;
        Console.CursorTop = top;
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