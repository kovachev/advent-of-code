using System.Security.Cryptography;
using System.Text;

namespace aoc_2015_04;

internal class Program
{
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 04");
        
        var input = "iwrupvqb";
        
        var number = isPart1 ? 0 : 346386;
        
        using (var md5 = MD5.Create())
        {
            while (true)
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{input}{number}"));
                
                var found = isPart1
                    ? hash[0] == 0 && hash[1] == 0 && hash[2] < 16
                    : hash[0] == 0 && hash[1] == 0 && hash[2] == 0;
                
                if (found)
                {
                    Console.WriteLine($"Number: {number}");
                    Console.WriteLine($"Hash: {ByteArrayToString(hash)}");
                    break;
                }
                
                number++;
            }
        }
    }
    
    private static string ByteArrayToString(byte[] input)
    {
        var hex = new StringBuilder(input.Length * 2);
        
        foreach (var b in input)
        {
            hex.Append($"{b:X2}");
        }
        
        return hex.ToString();
    }
}