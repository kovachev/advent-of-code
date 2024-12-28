using Helpers;

namespace aoc_2024_20;

internal class Program
{
    private const char WallMarker = '#';
    private const char StartMarker = 'S';
    private const char EndMarker = 'E';
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 20");
        
        Process("sample.txt", 2, 2);
        
        Process("input.txt", 2);
        
        Process("sample.txt", 20, 2);
        
        Process("input.txt", 20);
    }
    
    private static void Process(string inputFile, int cheatSize, int minSaving = 100)
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
        
        var (path, _) = pathAndScore;
        
        Console.WriteLine($"[{inputFile}] Path found with score {pathAndScore.Score}.");

        var count = 0;
        
        Parallel.For(0, path.Length, i => Interlocked.Add(ref count, GetCheats(i, path, cheatSize, minSaving)));
        
        Console.WriteLine($"[{inputFile}] Count of savings of {minSaving} or more with {cheatSize} cheat size: {count}");
    }

    private static int GetCheats(int currentPositionIndex, Position[] path, int cheatSize, int minSaving)
    {
        var count = 0;

        for (var prevPositionIndex = 0; prevPositionIndex < currentPositionIndex; prevPositionIndex++)
        {
            var dist = path[currentPositionIndex].ManhattanDistance(path[prevPositionIndex]);
            var saving = currentPositionIndex - (prevPositionIndex + dist);
            if (dist <= cheatSize && saving >= minSaving)
            {
                count++;
            }
        }

        return count;
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

        while (queue.TryDequeue(out var current, out var score))
        {
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