namespace aoc_2024_01;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Advent of Code 2024 - Day 1");
        
        var input = File.ReadAllLines("input.txt");

        var list1 = new List<int>();
        var list2 = new List<int>();
        
        foreach (var line in input)
        {
            var split = line.Split(' ');
            list1.Add(int.Parse(split.First()));
            list2.Add(int.Parse(split.Last()));
        }
        
        list1.Sort();
        list2.Sort();
        
        if (list1.Count != list2.Count)
        {
            Console.WriteLine("Error: Lists are not the same size");
            return;
        }
        
        var sum = 0;
        var sim = 0;
        for (var i = 0; i < list1.Count; i++)
        {
            sum += Math.Abs(list1[i] - list2[i]);
            var count = list2.Count(x => x == list1[i]);
            sim += list1[i] * count;
        }
        
        Console.WriteLine($"Sum of differences: {sum}");
        Console.WriteLine($"Sum of similarities: {sim}");
    }
}