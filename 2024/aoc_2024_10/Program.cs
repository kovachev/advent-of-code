using Helpers;

namespace aoc_2024_10;

internal class Program
{
    private const char StartMarker = '0';
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 10");
        
        var map = new Map("input.txt");
        //var map = new Map("sample.txt");

        var startingPositions = map.Where(x => x.Value == StartMarker)
                                   .Select(x => x.Position)
                                   .ToArray();
        
        var sumPart1 = 0;
        var sumPart2 = 0;
        foreach (var startingPosition in startingPositions)
        {
            var queue = new Queue<Position>();
            queue.Enqueue(startingPosition);
            
            var visited = new List<Position>();
            
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var currentValue = int.Parse(map[current].ToString());
                
                foreach (var neighbour in map.GetNeighbours(current))
                {
                    var neighbourValue = int.Parse(map[neighbour].ToString());
                    
                    var diff = neighbourValue - currentValue;

                    if (diff == 1)
                    {
                        if (neighbourValue == 9)
                        {
                            visited.Add(neighbour with { Parent = current });
                            continue;
                        }

                        queue.Enqueue(neighbour with { Parent = current });
                    }
                }
            }

            // foreach (var position in visited)
            // {
            //     // print path
            //     var current = position;
            //     while (current != null)
            //     {
            //         Console.Write(current);
            //         Console.Write(" <- ");
            //         current = current.Parent;
            //     }
            //     Console.WriteLine();
            //     Console.WriteLine();
            // }
            
            sumPart1 += visited.DistinctBy(x => new {x.X, x.Y}).Count();
            sumPart2 += visited.Count();
        }   
        
        Console.WriteLine($"Part 1: {sumPart1}");
        Console.WriteLine($"Part 2: {sumPart2}");
    }
}