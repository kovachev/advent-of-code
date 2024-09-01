using aoc.helpers;

namespace aoc_2015_09;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 09");
        
        var input = File.ReadAllLines("input.txt");
        
        var routes = new List<Route>();
        
        foreach (var line in input)
        {
            var parts = line.Split(" ");
            
            routes.Add(new Route(parts[0], parts[2], int.Parse(parts[4])));
        }
        
        var cities = routes.SelectMany(r => new[] { r.From, r.To })
                           .Distinct()
                           .ToArray();

        var distances = new Dictionary<string, int>();
        
        foreach (var permutation in Permutations.Permute(cities))
        {
            var distance = 0;
            
            for (var i = 0; i < permutation.Count - 1; i++)
            {
                var from = permutation[i];
                var to = permutation[i + 1];
                
                var route = routes.FirstOrDefault(r => r.From == from && r.To == to) ??
                            routes.First(r => r.From == to && r.To == from);
                
                distance += route.Distance;
            }
            
            distances.Add(string.Join(" -> ", permutation), distance);
        }
        
        Console.WriteLine($"Shortest distance: {distances.Values.Min()}");
        Console.WriteLine($"Longest distance: {distances.Values.Max()}");
    }
}