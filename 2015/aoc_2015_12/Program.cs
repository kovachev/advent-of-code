using System.Text.Json;
using System.Text.Json.Nodes;

namespace aoc_2015_12;

internal class Program
{
    internal static void Main(string[] args)
    {
        Console.WriteLine("Advent of Code 2015 - 12");
        
        var input = File.ReadAllText("input.json");

        var jsonNode = JsonNode.Parse(input);
        
        var sumPart1 = RawJsonSum(jsonNode, false);
        
        Console.WriteLine($"Part 1 Sum: {sumPart1}");
        
        var sumPart2 = RawJsonSum(jsonNode, true);
        
        Console.WriteLine($"Part 2 Sum: {sumPart2}");
    }
    
    private static int RawJsonSum(JsonNode? node, bool ignoreRed)
    {
        if (node == null)
        {
            return 0;
        }
        
        if (node.GetValueKind() == JsonValueKind.Number)
        {
            return node.GetValue<int>();
        }

        if (node.GetValueKind() == JsonValueKind.Array)
        {
            return node.AsArray()
                       .Select(x => RawJsonSum(x, ignoreRed))
                       .Sum();
        }

        if (node.GetValueKind() == JsonValueKind.Object)
        {
            if (ignoreRed &&
                node.AsObject()
                    .Any(x => x.Value?.GetValueKind() == JsonValueKind.String && 
                              x.Value.GetValue<string>() == "red"))
            {
                return 0;
            }

            return node.AsObject()
                       .Select(x => RawJsonSum(x.Value, ignoreRed))
                       .Sum();
        }

        return 0;
    }
}