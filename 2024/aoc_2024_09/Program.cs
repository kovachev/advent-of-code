namespace aoc_2024_09;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 9");
        
        //var input = File.ReadAllLines("input.txt");
        var input = File.ReadAllLines("sample.txt")
                        .First()
                        .AsSpan();
        
        var slots = new List<char[]>();
        
        for (var i = 0; i < input.Length; i++)
        {
            if (i % 2 == 0)
            {
                
            }
            else
            {
                
            }
        }
    }
}