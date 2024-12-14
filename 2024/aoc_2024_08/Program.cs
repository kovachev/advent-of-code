namespace aoc_2024_08;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 8");
        
        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample.txt");

        var map = input.Select(x => x.ToCharArray()).ToArray();

        var antennas = new Dictionary<char, List<Position>>();
        
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                var c = map[y][x];
                if (c == '.')
                {
                    continue;
                }
                
                if (!antennas.ContainsKey(c))
                {
                    antennas[c] = [];
                }

                antennas[c].Add(new Position(x, y));
            }
        }
        
        var antennaPairs = GetAntennaPairs(antennas).ToArray();

        var part1nodes = new List<Position>();
        var part2nodes = new List<Position>();
        
        foreach (var antennaPair in antennaPairs)
        {
            var (_, antenna1, antenna2) = antennaPair;
            
            var diff1 = antenna1 - antenna2;
            var diff2 = antenna2 - antenna1;

            if (!part2nodes.Contains(antenna1))
            {
                part2nodes.Add(antenna1);
            }
            
            if (!part2nodes.Contains(antenna2))
            {
                part2nodes.Add(antenna2);
            }
            
            var node1 = antenna1 + diff1;
            if (node1.IsOnMap(map) && !part1nodes.Contains(node1))
            {
                part1nodes.Add(node1);
            }
            while(node1.IsOnMap(map))
            {  
                if (part2nodes.Contains(node1))
                {
                    node1 += diff1;
                    continue;
                }
                part2nodes.Add(node1);
                node1 += diff1;
            }
            
            var node2 = antenna2 + diff2;
            if (node2.IsOnMap(map) && !part1nodes.Contains(node2))
            {
                part1nodes.Add(node2);
            }
            while(node2.IsOnMap(map))
            {  
                if (part2nodes.Contains(node2))
                {
                    node2 += diff2;
                    continue;
                }
                part2nodes.Add(node2);
                node2 += diff2;
            }
        }
        
        DrawMap(map, part1nodes);
        Console.WriteLine($"Part 1: {part1nodes.Count}");
        
        Console.WriteLine();
        Console.WriteLine();
        
        DrawMap(map, part2nodes);
        Console.WriteLine($"Part 2: {part2nodes.Count}");
    }

    private static void DrawMap(char[][] map, List<Position> antinodes)
    {
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                var c = map[y][x];
                if (antinodes.Contains(new Position(x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(c == '.' ? '#' : c);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(c);
                }
            }
            Console.WriteLine();
        }
    }
    
    private static IEnumerable<(char antennaType, Position antenna1, Position antenna2)> GetAntennaPairs(Dictionary<char, List<Position>> antennasByType)
    {
        foreach (var (antennaType, antennas) in antennasByType)  
        {
            for (var i = 0; i < antennas.Count - 1; i++)
            {
                for (var j = i + 1; j < antennas.Count; j++)
                {
                    yield return (antennaType, antennas[i], antennas[j]);
                }
            }
        }
    }
}

internal record Position(int X, int Y)
{
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    
    public bool IsOnMap(char[][] map) => IsValid(map[0].Length, map.Length);
    
    private bool IsValid(int width, int height) => X >= 0 && X < width && Y >= 0 && Y < height;
    
    public override string ToString() => $"({X}, {Y})";
}