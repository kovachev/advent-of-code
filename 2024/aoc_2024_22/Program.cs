namespace aoc_2024_22;

using Quadruple = (long, long, long, long);

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 22");

        var sample = File.ReadAllLines("sample.txt").Select(long.Parse).ToArray();

        Console.WriteLine($"Part 1 (sample): {Part1(sample)}");
        
        var sample2 = File.ReadAllLines("sample2.txt").Select(long.Parse).ToArray();
        
        //Console.WriteLine($"Part 2 (sample2): {Part2([1])}");
        
        Console.WriteLine($"Part 2 (sample2): {Part2(sample2)}");
        
        var input = File.ReadAllLines("input.txt").Select(long.Parse).ToArray();
        
        Console.WriteLine($"Part 1 (input): {Part1(input)}");
        
        Console.WriteLine($"Part 2 (input): {Part2(input)}");
    }

    private static long Part1(long[] seeds)
    {
        return seeds.Select(x => Sequence(x).Skip(2000).Take(1).Single()).Sum();
    }

    private static (Quadruple, long) Part2(long[] seeds)
    {
        var sequencesForSeed = new Dictionary<long, IDictionary<Quadruple, long>>();

        foreach (var seed in seeds)
        {
            sequencesForSeed[seed] = GetSequencesForSeed(seed);
        }

        var sequences = new Dictionary<Quadruple, long>();

        foreach (var seed in seeds)
        {
            foreach (var (key, value) in sequencesForSeed[seed])
            {
                sequences[key] = sequences.GetValueOrDefault(key) + value;
            }
        }

        var max = sequences.MaxBy(x => x.Value);
        
        return (max.Key, max.Value);
    }

    private static IDictionary<Quadruple, long> GetSequencesForSeed(long seed)
    {
        var sequences = new Dictionary<Quadruple, long>();
        
        var sequence = Sequence(seed).Take(2000)
                                     .Select(x => x % 10)
                                     .ToArray();

        var diff = sequence.Zip(sequence.Skip(1))
                           .Select(x => x.Second - x.First)
                           .ToArray();
            
        for (var i = 3; i < diff.Length; i++)
        {
            var key = (diff[i - 3], diff[i - 2], diff[i - 1], diff[i]);
            var value = sequence[i + 1];

            sequences.TryAdd(key, value);
        }

        return sequences;
    }
    
    private static IEnumerable<long> Sequence(long seed)
    {
        var value = seed;
        yield return value;
        
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