namespace aoc_2024_10;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 10");
        
        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample.txt");

        var map = input.Select(x => x.ToCharArray()).ToArray();

        var startingPositions = new List<Position>();
        
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '0')
                {
                    startingPositions.Add(new Position(x, y));
                }
            }
        }
        
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

                var currentValue = int.Parse(map[current.Y][current.X].ToString());
                
                foreach (var neighbour in GetNeighbours(current))
                {
                    if (!neighbour.IsOnMap(map))
                    {
                        continue;
                    }

                    var neighbourValue = int.Parse(map[neighbour.Y][neighbour.X].ToString());
                    
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
    
    private static IEnumerable<Position> GetNeighbours(Position position)
    {
        yield return position + new Position(0, 1);
        yield return position + new Position(1, 0);
        yield return position + new Position(0, -1);
        yield return position + new Position(-1, 0);
    }
}

internal record Position(int X, int Y, Position Parent = null)
{
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    
    public bool IsOnMap(char[][] map) => X >= 0 && X < map[0].Length && Y >= 0 && Y < map.Length;

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}