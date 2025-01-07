namespace aoc_2024_22;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 22");

        var sample = File.ReadAllLines("sample.txt").Select(long.Parse).ToArray();

        Console.WriteLine($"Part 1 (sample): {Part1(sample)}");
        
        var input = File.ReadAllLines("input.txt").Select(long.Parse).ToArray();
        
        Console.WriteLine($"Part 1 (input): {Part1(input)}");
    }

    private static long Part1(long[] sample)
    {
        return sample.Select(x => Sequence(x).Skip(1999).Take(1).Single()).Sum();
    }
    
    private static IEnumerable<long> Sequence(long seed)
    {
        var value = seed;
        while (true)
        {
            value = Prune(Mix(value * 64, value));
            value = Prune(Mix(value / 32, value));
            value = Prune(Mix(value * 2048, value));
            yield return value;
        }
    }
    
    private static long Mix(long value, long secret)
    {
        return value ^ secret;
    }
    
    private static long Prune(long secret)
    {
        return secret % 16777216L;
    }
}