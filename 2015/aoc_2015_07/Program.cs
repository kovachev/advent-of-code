namespace aoc_2015_07;

internal class Program
{
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 07");
        
        var variables = new Dictionary<string, ushort>();
        
        var input = File.ReadAllLines(isPart1 ? "input.txt" : "input_part2.txt");

        var instructions = input.Select(x => new Instruction(x)).ToArray();

        var skip = 0;
        while (instructions.Any(x => !x.IsExecuted))
        {
            var instruction = instructions.Skip(skip++).First(x => !x.IsExecuted);
            instruction.Execute(ref variables);

            if (instruction.IsExecuted)
            {
                skip = 0;
                //PrintVariables(ref variables);
                
                if (variables.ContainsKey("a"))
                {
                    break;
                }
            }
        }

        Console.WriteLine($"Value of wire 'a': {variables["a"]}");
    }
    
    // ReSharper disable once UnusedMember.Local
    private static void PrintVariables(ref Dictionary<string, ushort> variables)
    {
        Console.Clear();
        
        Console.WriteLine("Variables:");
        foreach (var (key, value) in variables)
        {
            Console.WriteLine($"{key}: {value}");
        }
    }
}