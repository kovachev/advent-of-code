namespace aoc_2024_17;

public class Program
{
    private static readonly Dictionary<int, Func<ProcessorState, ProcessorState>> _operations = new()
    {
        { 0, Adv },
        { 1, Bxl },
        { 2, Bst },
        { 3, Jnz },
        { 4, Bxc },
        { 5, Out },
        { 6, Bdv },
        { 7, Cdv }
    };
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 17");
        
        var sample = new ProcessorState(0, 729, 0, 0, [0, 1, 5, 4, 3, 0]);
        
        var input = new ProcessorState(0, 44348299, 0, 0, [2, 4, 1, 5, 7, 5, 1, 6, 0, 3, 4, 2, 5, 5, 3, 0]);
        
        Console.WriteLine($"Part 1 (sample): {Part1(sample).Output}");
        
        Console.WriteLine($"Part 1 (input): {Part1(input).Output}");
        
        var sample2 = new ProcessorState(0, 117440, 0, 0, [0, 3, 5, 4, 3, 0]);
        
        Console.WriteLine($"Part 1 (sample2): {Part1(sample2).Output}");
    }

    private static ProcessorState Part1(ProcessorState processorState)
    {
        var state = processorState with { };
        
        while (state.CanExecute)
        {
            state = _operations[state.Instructions[state.InstructionPointer]](state);
        }

        return state;
    }

    private static ProcessorState Adv(ProcessorState processorState)
    {
        var value = GetValue(processorState);

        var result = (int) (processorState.A / Math.Pow(2, value));
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            A = result
        };
    }
    
    private static ProcessorState Bxl(ProcessorState processorState)
    {
        var value = processorState.Instructions[processorState.InstructionPointer + 1];

        var result = processorState.B ^ value;
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            B = result
        };
    }
    
    private static ProcessorState Bst(ProcessorState processorState)
    {
        var value = GetValue(processorState);

        var result = value % 8;
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            B = result
        };
    }

    private static ProcessorState Jnz(ProcessorState processorState)
    {
        if (processorState.A == 0)
        {
            return processorState with { InstructionPointer = processorState.InstructionPointer + 2 };
        }

        var value = processorState.Instructions[processorState.InstructionPointer + 1];
        
        return processorState with { InstructionPointer = value };
    }

    private static ProcessorState Bxc(ProcessorState processorState)
    {
        var result = processorState.B ^ processorState.C;
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            B = result
        };
    }
    
    private static ProcessorState Out(ProcessorState processorState)
    {
        var value = GetValue(processorState);

        var output = (value % 8).ToString();

        if (processorState.Output.Length > 0)
        {
            output = $"{processorState.Output},{output}";
        }
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            Output = output
        };
    }
    
    private static ProcessorState Bdv(ProcessorState processorState)
    {
        var value = GetValue(processorState);

        var result = processorState.A / Math.Pow(2, value);
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            B = (int)result
        };
    }
    
    private static ProcessorState Cdv(ProcessorState processorState)
    {
        var value = GetValue(processorState);

        var result = processorState.A / Math.Pow(2, value);
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            C = (int)result
        };
    }
    private static int GetValue(ProcessorState processorState)
    {
        var operand = processorState.Instructions[processorState.InstructionPointer + 1];
        
        return operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => processorState.A,
            5 => processorState.B,
            6 => processorState.C,
            _ => throw new Exception("Invalid state")
        };
    }
}

internal record ProcessorState(
    int InstructionPointer,
    int A,
    int B,
    int C,
    int[] Instructions,
    string Output = "")
{
    public bool CanExecute => InstructionPointer < Instructions.Length;
};