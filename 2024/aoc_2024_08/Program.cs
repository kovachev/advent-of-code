using Helpers;

namespace aoc_2024_08;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 8");
        
        var map = new Map("input.txt");
        //var map = new Map("sample.txt");

        var antennas = map.Where(x => x.Value != '.')
                          .GroupBy(x => x.Value)
                          .ToDictionary(
                              x => x.Key,
                              x => x.Select(y => y.Position)
                                    .ToArray()
                          );
        
        var antennaPairs = GetAntennaPairs(antennas).ToArray();

        var part1Nodes = new List<Position>();
        var part2Nodes = new List<Position>();
        
        foreach (var antennaPair in antennaPairs)
        {
            var (_, antenna1, antenna2) = antennaPair;
            
            var diff1 = antenna1 - antenna2;
            var diff2 = antenna2 - antenna1;

            if (!part2Nodes.Contains(antenna1))
            {
                part2Nodes.Add(antenna1);
            }
            
            if (!part2Nodes.Contains(antenna2))
            {
                part2Nodes.Add(antenna2);
            }
            
            var node1 = antenna1 + diff1;
            if (map.IsOnMap(node1) && !part1Nodes.Contains(node1))
            {
                part1Nodes.Add(node1);
            }
            
            while(map.IsOnMap(node1))
            {  
                if (part2Nodes.Contains(node1))
                {
                    node1 += diff1;
                    continue;
                }
                part2Nodes.Add(node1);
                node1 += diff1;
            }
            
            var node2 = antenna2 + diff2;
            if (map.IsOnMap(node2) && !part1Nodes.Contains(node2))
            {
                part1Nodes.Add(node2);
            }
            while(map.IsOnMap(node2))
            {  
                if (part2Nodes.Contains(node2))
                {
                    node2 += diff2;
                    continue;
                }
                part2Nodes.Add(node2);
                node2 += diff2;
            }
        }
        
        DrawMap(map, part1Nodes);
        Console.WriteLine($"Part 1: {part1Nodes.Count}");
        
        Console.WriteLine();
        Console.WriteLine();
        
        DrawMap(map, part2Nodes);
        Console.WriteLine($"Part 2: {part2Nodes.Count}");
    }

    private static void DrawMap(Map map, List<Position> antinodes)
    {
        for (var y = 0; y < map.YMax; y++)
        {
            for (var x = 0; x < map.XMax; x++)
            {
                var c = map[x, y];
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
    
    private static IEnumerable<(char antennaType, Position antenna1, Position antenna2)> GetAntennaPairs(Dictionary<char, Position[]> antennasByType)
    {
        foreach (var (antennaType, antennas) in antennasByType)  
        {
            for (var i = 0; i < antennas.Length - 1; i++)
            {
                for (var j = i + 1; j < antennas.Length; j++)
                {
                    yield return (antennaType, antennas[i], antennas[j]);
                }
            }
        }
    }
}