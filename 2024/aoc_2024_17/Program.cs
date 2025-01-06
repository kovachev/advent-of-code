using System.Text;

namespace aoc_2024_17;

public class Program
{
    private static readonly Dictionary<long, Func<ProcessorState, ProcessorState>> _operations = new()
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
        
        Console.WriteLine($"Part 1 (sample): {ExecuteProgram(sample).Output}");
        
        Console.WriteLine($"Part 1 (input): {ExecuteProgram(input).Output}");
        
        var sample2 = new ProcessorState(0, 117440, 0, 0, [0, 3, 5, 4, 3, 0]);
        
        Console.WriteLine($"Part 1 (sample2): {ExecuteProgram(sample2).Output}");
        
        Console.WriteLine();
        Console.WriteLine(ToHumanReadableCode(input.Instructions));
        
        Part2(input);
    }

    private static ProcessorState ExecuteProgram(ProcessorState processorState)
    {
        var state = processorState with { };
        
        while (state.CanExecute)
        {
            state = _operations[state.Instructions[state.InstructionPointer]](state);
        }

        return state;
    }
    
    private static void Part2(ProcessorState processorState)
    {
        var possibleValues = ReverseModulo(processorState.Instructions, processorState.Instructions).ToArray();
        
        Console.WriteLine($"Part 2: {possibleValues.Min()}");
    }

    private static IEnumerable<long> ReverseModulo(long[] instructions, long[] output)
    {
        if (!output.Any())
        {
            yield return 0;
            yield break;
        }

        foreach (var value in ReverseModulo(instructions, output[1..]))
        {
            var expectedOutput = string.Join(",", output);
            
            for (var remainder = 0; remainder < 8; remainder++)
            {
                var a = value * 8 + remainder;
                
                var processorState = ExecuteProgram(new ProcessorState(0, a, 0, 0, instructions.ToArray()));
                if (processorState.Output == expectedOutput)
                {
                    yield return a;
                }
            }
        }
    }

    private static string ToHumanReadableCode(long[] instructions)
    {
        var result = new StringBuilder();
        
        for (var i = 0; i < instructions.Length; i += 2)
        {
            var opcode = instructions[i] switch
            {
                0 => "ADV",
                1 => "BXL",
                2 => "BST",
                3 => "JNZ",
                4 => "BXC",
                5 => "OUT",
                6 => "BDV",
                7 => "CDV",
                _ => throw new ArgumentOutOfRangeException()
            };

            var operand = instructions[i + 1] switch
            {
                0 => "0",
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "A",
                5 => "B",
                6 => "C",
                _ => throw new ArgumentOutOfRangeException()
            };

            if (instructions[i] is 1 or 3)
            {
                operand = instructions[i + 1].ToString();
            } 
            else if (instructions[i] is 4)
            {
                operand = "";
            }
            
            result.AppendLine($"{opcode} {operand}");
        }

        return result.ToString();
    }

    private static ProcessorState Adv(ProcessorState processorState)
    {
        var value = GetValue(processorState);

        var result = (long) (processorState.A / Math.Pow(2, value));
        
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
            B = (long)result
        };
    }
    
    private static ProcessorState Cdv(ProcessorState processorState)
    {
        var value = GetValue(processorState);

        var result = processorState.A / Math.Pow(2, value);
        
        return processorState with
        {
            InstructionPointer = processorState.InstructionPointer + 2,
            C = (long)result
        };
    }
    private static long GetValue(ProcessorState processorState)
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
            _ => throw new InvalidStateException()
        };
    }
}

internal record ProcessorState(
    long InstructionPointer,
    long A,
    long B,
    long C,
    long[] Instructions,
    string Output = "")
{
    public bool CanExecute => InstructionPointer < Instructions.Length;
};

internal class InvalidStateException : Exception
{
    public InvalidStateException() : base("Processor is in an invalid state.")
    {
    }
}