using System.Drawing;

namespace aoc_2024_16;

internal class Program
{
    private const char StartMark = 'S';
    private const char EndMark = 'E';
    private const char WallMark = '#';
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 16");
        
        //var input = File.ReadAllLines("input.txt");
        var input = File.ReadAllLines("sample1.txt");
        //var input = File.ReadAllLines("sample2.txt");

        var map = input.Select(x => x.ToCharArray()).ToArray();
        
        var startPosition = FindPositions(map, StartMark);
        var endPosition = FindPositions(map, EndMark);
        
        Console.WriteLine($"Looking for path from {startPosition} to {endPosition}");
        
        var paths = FindPaths(map, startPosition, endPosition);
        
        foreach (var path in paths)
        {
            Console.WriteLine(string.Join(" -> ", path));
        }
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

    private static IEnumerable<IEnumerable<Position>> FindPaths(char[][] map, Position startPosition, Position endPosition)
    {
        var queue = new Queue<Position>();
        queue.Enqueue(startPosition);
            
        var endReached = new List<Position>();
            
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var path = ExtractPath(current, reverse: true);
            
            foreach (var neighbour in GetNeighbours(current))
            {
                if (!neighbour.IsOnMap(map) ||
                    map[neighbour.Y][neighbour.X] == WallMark ||
                    path.Contains(neighbour))
                {
                    continue;
                }

                if (neighbour == endPosition)
                {
                    endReached.Add(neighbour with { Parent = current });
                    continue;
                }

                queue.Enqueue(neighbour with { Parent = current });
            }

            var pathWithColor = path.Select(p => (p, ConsoleColor.Green)).ToList();
            pathWithColor[0] = (pathWithColor[0].Item1, ConsoleColor.Red);
            PrintMap(map, pathWithColor);
            Thread.Sleep(500);
        }

        var paths = new List<List<Position>>();
        
        foreach (var position in endReached)
        {
            var path = ExtractPath(position, reverse: true);
            paths.Add(path.ToList());
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
    
    private static void PrintMap(char[][] map, IEnumerable<(Position, ConsoleColor)>? positions = null)
    {
        Console.Clear();
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                var position = new Position(x, y);
                if (positions?.Select(p => p.Item1).Contains(position) == true)
                {
                    Console.BackgroundColor = positions.Single(p => p.Item1 == position).Item2;
                }

                Console.Write(map[y][x]);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}

internal record Position(int X, int Y, Position? Parent = null)
{
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    
    public bool IsOnMap(char[][] map) => X >= 0 && X < map[0].Length && Y >= 0 && Y < map.Length;
    
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}