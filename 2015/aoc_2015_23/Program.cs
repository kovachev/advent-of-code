namespace aoc_2015_23;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 23");
        
        var input = File.ReadAllLines("input.txt");
        
        var instructions = input.Select(ParseInstruction)
                                .ToArray();
        
        var part1Computer = new Computer();
        part1Computer.Execute(instructions);
        
        Console.WriteLine("Part 1: b = {0}", part1Computer.GetRegisterValue("b"));
        
        var part2Computer = new Computer(new KeyValuePair<string, uint>("a", 1));
        part2Computer.Execute(instructions);
        
        Console.WriteLine("Part 2: b = {0}", part2Computer.GetRegisterValue("b"));
    }
    
    private static Instruction ParseInstruction(string line)
    {
        var parts = line.Split(' ');
        
        var operation = parts[0];
        
        var register = parts[1].TrimEnd(',');
        
        var offset = parts.Length == 3 ? int.Parse(parts[2]) : 0;
        
        if (operation == Operation.Jump)
        {
            offset = int.Parse(register);
            register = "";
        }
        
        return new Instruction(operation, register, offset);
    }
}