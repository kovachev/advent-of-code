namespace aoc_2024_17;

public class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 17");
        
        var sample = new Processor(0, 729, 0, 0, [0, 1, 5, 4, 3, 0]);

        var input = new Processor(0, 44348299, 0, 0, [2, 4, 1, 5, 7, 5, 1, 6, 0, 3, 4, 2, 5, 5, 3, 0]);
        
        Dictionary<int, Func<Processor, Processor>> operations = new()
        {
            {0, Adv},
            {1, Bxl},
            {2, Bst},
            {3, Jnz},
            {4, Bxc},
            {5, Out},
            {6, Bdv},
            {7, Cdv}
        };

        while (sample.CanExecute)
        {
            sample = operations[sample.Instructions[sample.InstructionPointer]](sample);
        }
    }

    private static Processor Adv(Processor processor)
    {
        var value = GetValue(processor);

        var result = processor.A / Math.Pow(2, value);
        
        return processor with
        {
            InstructionPointer = processor.InstructionPointer + 2,
            A = (int) result
        };
    }
    
    private static Processor Bxl(Processor processor)
    {
        var value = processor.Instructions[processor.InstructionPointer + 1];

        var result = processor.B ^ value;
        
        return processor with
        {
            InstructionPointer = processor.InstructionPointer + 2,
            B = result
        };
    }
    
    private static Processor Bst(Processor processor)
    {
        var value = GetValue(processor);

        var result = value % 8;
        
        return processor with
        {
            InstructionPointer = processor.InstructionPointer + 2,
            B = result
        };
    }

    private static Processor Jnz(Processor processor)
    {
        if (processor.A == 0)
        {
            return processor with { InstructionPointer = processor.InstructionPointer + 2 };
        }

        var value = processor.Instructions[processor.InstructionPointer + 1];
        
        return processor with { InstructionPointer = processor.InstructionPointer + value };
    }

    private static Processor Bxc(Processor processor)
    {
        var result = processor.B ^ processor.C;
        
        return processor with
        {
            InstructionPointer = processor.InstructionPointer + 2,
            B = result
        };
    }
    
    private static Processor Out(Processor processor)
    {
        var value = GetValue(processor);

        var output = value % 8;
        
        Console.WriteLine(output + ", ");
        
        return processor with { InstructionPointer = processor.InstructionPointer + 2 };
    }
    
    private static Processor Bdv(Processor processor)
    {
        var value = GetValue(processor);

        var result = processor.A / Math.Pow(2, value);
        
        return processor with
        {
            InstructionPointer = processor.InstructionPointer + 2,
            B = (int)result
        };
    }
    
    private static Processor Cdv(Processor processor)
    {
        var value = GetValue(processor);

        var result = processor.A / Math.Pow(2, value);
        
        return processor with
        {
            InstructionPointer = processor.InstructionPointer + 2,
            C = (int)result
        };
    }
    private static int GetValue(Processor processor)
    {
        var operand = processor.Instructions[processor.InstructionPointer + 1];
        
        return operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => processor.A,
            5 => processor.B,
            6 => processor.C,
            _ => operand
        };
    }
}

internal record Processor(
    int InstructionPointer,
    int A,
    int B,
    int C,
    int[] Instructions)
{
    public bool CanExecute => InstructionPointer < Instructions.Length;
};