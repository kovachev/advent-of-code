namespace aoc_2015_23;

internal class Computer
{
    public uint GetRegisterValue(string register) => Registers[register];

    private IDictionary<string, uint> Registers { get; } = new Dictionary<string, uint>
    {
        ["a"] = 0,
        ["b"] = 0
    };

    private int _instructionPointer;

    public Computer()
    {
    }
    
    public Computer(params KeyValuePair<string, uint>[] registers)
    {
        foreach (var register in registers)
        {
            if (register.Key != "a" && register.Key != "b")
            {
                throw new ArgumentException("Invalid register name", nameof(registers));
            }

            Registers[register.Key] = register.Value;
        }
    }
    
    public void Execute(Instruction[] instructions)
    {
        _instructionPointer = 0;
        
        while (_instructionPointer >= 0 && _instructionPointer < instructions.Length)
        {
            var instruction = instructions[_instructionPointer];
            
            Console.WriteLine($"State: {string.Join(", ", Registers.Select(x => $"{x.Key} = {x.Value,10}"))} | [IP:{_instructionPointer,3}] {instruction.Operation} {instruction.Register} {(instruction.Offset == 0 ? "" : instruction.Offset.ToString())}");
            
            switch (instruction.Operation)
            {
                case Operation.Half:
                    Registers[instruction.Register] /= 2;
                    _instructionPointer++;
                    break;
                
                case Operation.Triple:
                    Registers[instruction.Register] *= 3;
                    _instructionPointer++;
                    break;
                
                case Operation.Increment:
                    Registers[instruction.Register]++;
                    _instructionPointer++;
                    break;
                
                case Operation.Jump:
                    _instructionPointer += instruction.Offset;
                    break;
                
                case Operation.JumpIfEven:
                    if (Registers[instruction.Register] % 2 == 0)
                    {
                        _instructionPointer += instruction.Offset;
                    }
                    else
                    {
                        _instructionPointer++;
                    }
                    break;
                
                case Operation.JumpIfOne:
                    if (Registers[instruction.Register] == 1)
                    {
                        _instructionPointer += instruction.Offset;
                    }
                    else
                    {
                        _instructionPointer++;
                    }
                    break;
            }
        }
    }
}