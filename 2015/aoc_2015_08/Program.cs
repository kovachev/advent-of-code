namespace aoc_2015_08;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 08");
        
        var input = File.ReadAllLines("input.txt");
        
        var entries = input.Select(x => new EscapedValue(x))
                           .ToArray();
        
        var totalCharacters = entries.Sum(x => x.DecodedCharacters);
        var totalDoubleEncodedCharacters = entries.Sum(x => x.DoubleEncodedCharacters);
        var totalLength = entries.Sum(x => x.Length);
        
        Console.WriteLine($"Part 1: {totalLength - totalCharacters}");
        Console.WriteLine($"Part 2: {totalDoubleEncodedCharacters - totalLength}");
    }
}