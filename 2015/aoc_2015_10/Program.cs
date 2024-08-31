using System.Text;

namespace aoc_2015_10;

internal class Program
{
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 10");
        
        var input = "3113322113";
        
        var iterations = isPart1 ? 40 : 50;
        
        for (var i = 0; i < iterations; i++)
        {
            input = LookAndSay(input);
        }
        
        Console.WriteLine($"Length of the result after {iterations} iterations: {input.Length}");
    }

    private static string LookAndSay(string input)
    {
        var result = new StringBuilder();
        
        var currentChar = input[0];
        var currentCharCount = 1;
        
        for (var i = 1; i < input.Length; i++)
        {
            if (input[i] == currentChar)
            {
                currentCharCount++;
            }
            else
            {
                result.Append(currentCharCount);
                result.Append(currentChar);
                
                currentChar = input[i];
                currentCharCount = 1;
            }
        }
        
        result.Append(currentCharCount);
        result.Append(currentChar);
        
        return result.ToString();
    }
}