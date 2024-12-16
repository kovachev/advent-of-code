namespace aoc_2024_11;

internal class Program
{
    private static readonly Dictionary<(string, int), long> _computed = new();
    private static long _cacheHits; 
    private static long _cacheMisses;
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 11");
        
        var input = File.ReadAllText("input.txt").Trim();

        var stones = input.Split(" ");
        
        Console.WriteLine($"Part 1: {stones.Sum(n => Blink(long.Parse(n), 25))}");
        Console.WriteLine($"Part 2: {stones.Sum(n => Blink(long.Parse(n), 75))}");
        Console.WriteLine();
        Console.WriteLine($"Cache hits: {_cacheHits}");
        Console.WriteLine($"Cache misses: {_cacheMisses}");
    }
    
    private static long Blink(long stone, int blinks)
    {
        var stoneStr = stone.ToString();
        if (_computed.TryGetValue((stoneStr, blinks), out var result))
        {
            _cacheHits++;
            return result;
        }
        
        _cacheMisses++;
        
        long count = (stoneStr, blinks) switch
        {
            (_, 0) => 1,
            
            ("0", _) => Blink(1, blinks - 1),
            
            var (s, _) when s.Length % 2 == 0 => Blink(long.Parse(s[..(s.Length / 2)]), blinks - 1) + Blink(long.Parse(s[(s.Length / 2)..]), blinks - 1),
            
            _ => Blink(2024 * stone, blinks - 1)
        };

        _computed[(stoneStr, blinks)] = count;
        
        return count;
    }
}