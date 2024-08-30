namespace aoc_2015_07;

internal class PlugBoard
{
    private readonly IDictionary<string, ushort> _variables = new Dictionary<string, ushort>();

    private readonly IEnumerable<Instruction> _instructions; 
    
    internal PlugBoard(IEnumerable<string> input)
    {
        _instructions = input.Select(x => new Instruction(x, this))
                            .ToArray();
    }
    
    internal void ExecuteInstructions()
    {
        const int maxIterations = 1000;
        
        var skip = 0;
        var iteration = 0;
        
        while (_instructions.Any(x => !x.IsExecuted) && iteration++ < maxIterations)
        {
            var instruction = _instructions.Skip(skip++)
                                           .First(x => !x.IsExecuted && x.CanExecute());
            
            instruction.Execute();

            if (instruction.IsExecuted)
            {
                skip = 0;
                //PrintVariables();
                
                if (_variables.ContainsKey("a"))
                {
                    break;
                }
            }
        }
    }
    
    internal ushort? GetValue(string variableName)
    {
        if (ushort.TryParse(variableName, out var value))
        {
            return value;
        }

        if (_variables.TryGetValue(variableName, out value))
        {
            return value;
        }

        return null;
    }
    
    internal void SetValue(string variableName, ushort value)
    {
        _variables.TryAdd(variableName, value);
    }
    
    // ReSharper disable once UnusedMember.Global
    internal void PrintVariables()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine();
        
        Console.WriteLine("Variables:");
        foreach (var (key, value) in _variables)
        {
            Console.WriteLine($"{key}: {value}");
        }
    }
}