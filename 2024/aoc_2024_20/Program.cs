using Helpers;

namespace aoc_2024_20;

internal class Program
{
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
        
        var startPosition = map.Single(x => x.Value == Map.StartMarker).Position;
        var endPosition = map.Single(x => x.Value == Map.EndMarker).Position;
        
        Console.WriteLine($"Looking for path from {startPosition} to {endPosition} on {map.XMax}x{map.YMax} map.");
        
        var pathAndScore = map.FindPath(startPosition, endPosition, debug: false);
        
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
}