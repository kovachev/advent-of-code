namespace aoc_2015_14;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 14");
        
        var input = File.ReadAllLines("input.txt");
        
        const int period = 2503;
        
        var score = Part1(input, period);
        Console.WriteLine($"Part 1: {score}");  
        
        score = Part2(input, period);
        Console.WriteLine($"Part 2: {score}");
    }
    
    private static int Part1(IEnumerable<string> input, int time)
    {
        var reindeers = input.Select(ParseReindeer)
                             .ToList();
        
        return reindeers.Max(r => r.Distance(time));
    }
    
    private static int Part2(IEnumerable<string> input, int time)
    {
        var reindeers = input.Select(ParseReindeer)
                             .ToList();
        
        var scores = new Dictionary<string, int>();
        
        for (var t = 1; t <= time; t++)
        {
            var distances = reindeers.ToDictionary(r => r.Name, r => r.Distance(t));
            
            var maxDistance = distances.Values
                                       .Max();
            
            foreach (var (name, distance) in distances)
            {
                if (distance == maxDistance)
                {
                    scores[name] = scores.GetValueOrDefault(name) + 1;
                }
            }
        }
        
        return scores.Values.Max();
    }
    
    private static Reindeer ParseReindeer(string line)
    {
        var parts = line.Split(' ');
        
        // Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
        // 0     1   2   3  4    5   6  7        8   9    10   11   12  13  14
        return new Reindeer(parts[0], int.Parse(parts[3]), int.Parse(parts[6]), int.Parse(parts[13]));
    }
}

internal record Reindeer(string Name, int Speed, int FlyTime, int RestTime)
{
    public int Distance(int time)
    {
        var cycleTime = FlyTime + RestTime;
        
        var cycles = time / cycleTime;
        
        var remainingTime = time % cycleTime;
        
        var flyTime = Math.Min(FlyTime, remainingTime);
        
        return cycles * FlyTime * Speed + flyTime * Speed;
    }
}