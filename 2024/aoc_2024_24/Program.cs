using System.Text.RegularExpressions;

namespace aoc_2024_24;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 24");
        
        var (values, operations) = Load("sample.txt");

        Part1(operations, values);
        
        Console.WriteLine();
        
        (values, operations) = Load("input.txt");
        
        Part1(operations, values);
    }

    private static void Part1(Operation[] operations, Dictionary<string, long> values)
    {
        while (operations.Any(x => x.result == null))
        {
            var canCompute = operations.Where(x => values.ContainsKey(x.a) && 
                                                   values.ContainsKey(x.b) && 
                                                   x.result == null)
                                       .ToArray();

            foreach (var operation in canCompute)
            {
                var a = values[operation.a];
                var b = values[operation.b];
                
                long result = operation.operation switch
                {
                    "AND" => a & b,
                    "OR" => a | b,
                    "XOR" => a ^ b,
                    _ => throw new InvalidOperationException()
                };
                
                values[operation.c] = result;

                var op = Array.IndexOf(operations, operation);
                operations[op] = operation with { result = result };
            }
        }

        var part1 = 0L;
        foreach (var (key, value) in values.Where(x => x.Key.StartsWith("z")).OrderBy(x => x.Key))
        {
            Console.WriteLine($"{key}: {value}");
            var shift = int.Parse(key[1..]);
            part1 |= value << shift;
        }
        
        Console.WriteLine($"Part 1: {part1}");
    }

    private static (Dictionary<string, long>, Operation[]) Load(string fileName)
    {
        var input = File.ReadAllLines(fileName);

        var valuesRegex = new Regex(@"^[a-zA-Z0-9]{3,3}: \d+$");
        var operationsRegex = new Regex("^([a-zA-Z0-9]{3,3}) (XOR|OR|AND) ([a-zA-Z0-9]{3,3}) -> ([a-zA-Z0-9]{3,3})$");
        
        var values = new Dictionary<string, long>();
        var operations = new List<Operation>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (valuesRegex.IsMatch(line))
            {
                var parts = line.Split(':');
                values[parts[0].Trim()] = long.Parse(parts[1].Trim());
                
                continue;
            }
            
            if (operationsRegex.IsMatch(line))
            {
                var match = operationsRegex.Match(line);
                operations.Add(new Operation(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value));
            }
        }
        
        return (values, operations.ToArray());
    }
}

internal record Operation(string a, string operation, string b, string c, long? result = null);