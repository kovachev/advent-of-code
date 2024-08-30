namespace aoc_2015_07;

internal class Program
{
    // Expected output:
    // Part 1: 956
    // Part 2: 40149
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 07");
        
        var input = File.ReadAllLines(isPart1 ? "input.txt" : "input_part2.txt");

        var plugBoard = new PlugBoard(input);
        
        plugBoard.ExecuteInstructions();

        Console.WriteLine($"Value of wire 'a': {plugBoard.GetValue("a")}");
    }
}