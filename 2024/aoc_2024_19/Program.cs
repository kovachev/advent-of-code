using System.Collections.Concurrent;

namespace aoc_2024_19;

internal class Program
{
    private static readonly ConcurrentDictionary<string, long> _cache = new();
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 19");
        
        Part1("sample.txt");
        
        Part1("input.txt");
    
        Part2("sample.txt");
        
        Part2("input.txt");
    }
    
    private static void Part1(string inputFile)
    {
        _cache.Clear();
        
        var (towels, designs) = ParseInput(inputFile);
        
        Console.WriteLine($"Part 1 {inputFile}: {designs.Count(x => CountTowels(towels, x) != 0)}");
    }
    
    private static void Part2(string inputFile)
    {
        _cache.Clear();
        
        var (towels, designs) = ParseInput(inputFile);
        
        Console.WriteLine($"Part 2 {inputFile}: {designs.Sum(x => CountTowels(towels, x))}");
    }
    
    private static (string[] Towels, string[] Designs) ParseInput(string inputFile)
    {
        var input = File.ReadAllText(inputFile);
        
        var parts = input.Split("\n\n");
        
        var towels = parts[0].Split(',')
                             .Select(x => x.Trim())
                             .ToArray();
        
        var designs = parts[1].Split('\n')
                              .Select(x => x.Trim())
                              .ToArray();
        
        return (towels, designs);
    }
    
    private static long CountTowels(string[] towels, string design)
    {
        return _cache.GetOrAdd(design, d =>
        {
            return d switch
            {
                "" => 1,
                _ => towels.Where(d.StartsWith)
                           .Sum(t => CountTowels(towels, d[t.Length..]))
            };
        });
    }
}