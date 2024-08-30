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
        
        foreach (var permutation in Permute(cities))
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

    private static IList<IList<string>> Permute(string[] cities)
    {
        var list = new List<IList<string>>();
        return DoPermute(cities, 0, cities.Length - 1, list);
    }

    private static IList<IList<string>> DoPermute(string[] cities, int start, int end, IList<IList<string>> list)
    {
        if (start == end)
        {
            // We have one of our possible n! solutions,
            // add it to the list.
            list.Add(new List<string>(cities));
        }
        else
        {
            for (var i = start; i <= end; i++)
            {
                Swap(ref cities[start], ref cities[i]);
                DoPermute(cities, start + 1, end, list);
                Swap(ref cities[start], ref cities[i]);
            }
        }

        return list;
    }

    private static void Swap(ref string a, ref string b)
    {
        (a, b) = (b, a);
    }
}