using Helpers;

namespace aoc_2024_12;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 12");
        
        var map = new Map("input.txt");
        //var map = new Map("sample1.txt");
        //var map = new Map("sample2.txt");
        //var map = new Map("sample3.txt");

        var regions = new List<Region>();

        for (var y = 0; y < map.YMax; y++)
        {
            for (var x = 0; x < map.XMax; x++)
            {
                var position = new Position(x, y);
                if (!regions.Any(r => r.Contains(position)))
                {
                    var newRegion = Traverse(map, position);
                    regions.Add(newRegion);
                }
            }
        }

        // foreach (var region in regions)
        // {
        //     PrintMap(map, region);
        //     Console.WriteLine($"Region {region.Type} has area {region.Area}, perimeter {region.Perimeter(map)} and edges {region.Edges(map)}");
        //     Console.WriteLine();
        //     Console.ReadLine();
        // }
        
        var price1 = regions.Sum(r => r.Area * r.Perimeter(map));
        
        Console.WriteLine($"Part 1: {price1}");
        
        var price2 = regions.Sum(r => r.Area * r.Edges(map));
        
        Console.WriteLine($"Part 2: {price2}");
    }

    private static Region Traverse(Map map, Position start)
    {
        var type = map[start];
        
        var positionsQueue = new Queue<Position>();
        
        positionsQueue.Enqueue(start);
        
        var visited = new List<Position> {start};
        
        while (positionsQueue.Count > 0)
        {
            var current = positionsQueue.Dequeue();
            
            foreach (var neighbor in map.GetNeighbours(current,  c => c == type))
            {
                if (visited.Contains(neighbor))
                {
                    continue;
                }

                visited.Add(neighbor);
                
                positionsQueue.Enqueue(neighbor);
            }
        }
        
        return new Region(type, visited.ToArray());
    }

    private static void PrintMap(Map map, Region region)
    {
        for (var y = 0; y < map.YMax; y++)
        {
            for (var x = 0; x < map.XMax; x++)
            {
                var position = new Position(x, y);
                if (region.Contains(position))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                Console.Write(map[position]);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}

internal record Region(char Type, Position[] Content)
{
    public bool Contains(Position position)
    {
        return Content.Any(x => x.X == position.X && x.Y == position.Y);
    }
    
    public int Area => Content.Length;

    public int Perimeter(Map map)
    {
        var perimeter = 0;
        
        foreach (var position in Content)
        {
            foreach (var direction in new[] { Position.Up, Position.Down, Position.Left, Position.Right })
            {
                var neighbor = position + direction;
                if (map.GetOrDefault(neighbor, ' ') != Type)
                {
                    perimeter++;
                }
            }
        }
        
        return perimeter;
    }

    public int Edges(Map map)
    {
        var edges = 0;
        
        foreach (var position in Content)
        {
            foreach (var direction in new[] { (Position.Up, Position.Left), (Position.Up, Position.Right), 
                                              (Position.Down, Position.Left), (Position.Down, Position.Right) })
            {
                var n1 = position + direction.Item1;
                var n2 = position + direction.Item2;
                var n3 = position + direction.Item1 + direction.Item2;
                
                // Outer edges
                if ((map.GetOrDefault(n1, ' ') != Type) &&
                    (map.GetOrDefault(n2, ' ') != Type))
                {
                    edges++;
                }
                
                // Inner edges
                if ((map.GetOrDefault(n1, ' ') == Type) &&
                    (map.GetOrDefault(n2, ' ') == Type) &&
                    (map.GetOrDefault(n3, ' ') != Type))
                {
                    edges++;
                }   
            }
        }

        return edges;
    }
}; 