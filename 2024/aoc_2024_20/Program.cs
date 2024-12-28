using Helpers;

namespace aoc_2024_20;

internal class Program
{
    private const char WallMarker = '#';
    private const char EmptyMarker = '.';
    private const char StartMarker = 'S';
    private const char EndMarker = 'E';
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 20");
        
        //Part1("sample.txt");
        
        Part1("input.txt");
    }
    
    private static void Part1(string inputFile)
    {
        var map = new Map(inputFile);
        
        var startPosition = map.Single(x => x.Value == StartMarker).Position;
        var endPosition = map.Single(x => x.Value == EndMarker).Position;
        
        Console.WriteLine($"Looking for path from {startPosition} to {endPosition} on {map.XMax}x{map.YMax} map.");
        
        var pathAndScore = FindPath(map, startPosition, endPosition, debug: false);
        
        if (pathAndScore == null)
        {
            Console.WriteLine($"[{inputFile}] No path found.");
            return;
        }
        
        Console.WriteLine($"[{inputFile}] Path found with score {pathAndScore.Score}.");

        var count = 0;
        var index = 0;
        foreach (var position in pathAndScore.Path)
        {
            index++;
            Console.WriteLine($"[{inputFile}] Checking position {index}/{pathAndScore.Path.Length}.");
            
            var neighbourWalls = map.GetNeighbours(position)
                                    .Where(p => map[p] == WallMarker)
                                    .ToArray();
            
            foreach (var wall in neighbourWalls)
            {
                var mapCopy = map.Clone();
                mapCopy[wall] = EmptyMarker;
                var result = FindPath(mapCopy, startPosition, endPosition);
                if (result != null && 
                    (pathAndScore.Score - result.Score) >= 100)
                {
                    Console.WriteLine($"[{inputFile}] Found saving of 100 or more.");
                    count += 1;
                }
            }
        }
        
        Console.WriteLine($"[{inputFile}] Count of savings of 100 or more: {count}");
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
                    //ShowPath(map, pathWithColor);
                    Thread.Sleep(50);
                }
            }
        }
        
        return result;
    }
}