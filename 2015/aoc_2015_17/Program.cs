namespace aoc_2015_17;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 17");
        
        var containers = new[] { 11, 30, 47, 31, 32, 36, 3, 1, 5, 3, 32, 36, 15, 11, 46, 26, 28, 1, 19, 3 };

        Part1(containers);
        Part2(containers);
    }

    private static void Part1(IList<int> containers)
    {
        var count = 0;
        foreach (var combination in GetPowerSet(containers)) 
        {
            var sum = combination.Sum(x => x);
            if (sum == 150)
            {
                count++;
                Console.WriteLine($"Combination: {string.Join(", ", combination)}");
            }
        }
        
        Console.WriteLine($"Total combinations: {count}");
    }
    
    private static void Part2(IList<int> containers)
    {
        var count = 0;
        var minContainers = int.MaxValue;
        foreach (var combination in GetPowerSet(containers)) 
        {
            var sum = combination.Sum(x => x);
            if (sum == 150)
            {
                if (combination.Count() < minContainers)
                {
                    minContainers = combination.Count();
                    count = 1;
                    Console.WriteLine("=====================================");
                    Console.WriteLine($"Combination: {string.Join(", ", combination)}");
                }
                else if (combination.Count() == minContainers)
                {
                    count++;
                    Console.WriteLine($"Combination: {string.Join(", ", combination)}");
                }
            }
        }
        
        Console.WriteLine($"Total combinations: {count}");
        Console.WriteLine($"Minimum containers: {minContainers}");
    }
    
    private static IEnumerable<IEnumerable<T>> GetPowerSet<T>(IList<T> list)
    {
        return from m in Enumerable.Range(0, 1 << list.Count)
            select
                from i in Enumerable.Range(0, list.Count)
                where (m & (1 << i)) != 0
                select list[i];
    }
}