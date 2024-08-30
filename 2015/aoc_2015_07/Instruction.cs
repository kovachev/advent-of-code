namespace aoc_2015_07;

internal class Instruction
{
    private readonly string _operand1;
    private readonly string? _operand2;
    private readonly string _destination;
    private readonly Operation _operation;
    
    public bool IsExecuted { get; private set; }

    internal Instruction(string instruction)
    {
        var parts = instruction.Split(" ");
        
        if (parts.Length == 3)
        {
            // 123 -> x
            // x -> y
            _operation = Operation.Assign;
            _operand1 = parts[0];
            _operand2 = null;
            _destination = parts[2];
        }
        else if (parts.Length == 4)
        {
            // NOT x -> y
            _operation = Operation.Not;
            _operand1 = parts[1];
            _operand2 = null;
            _destination = parts[3];
        }
        else if (parts.Length == 5)
        {
            // x AND y -> z
            // x OR y -> z
            // x LSHIFT 2 -> z
            // x RSHIFT 2 -> z
            _operand1 = parts[0];
            _operand2 = parts[2];
            _destination = parts[4];
            
            _operation = parts[1] switch
            {
                "AND" => Operation.And,
                "OR" => Operation.Or,
                "LSHIFT" => Operation.LShift,
                "RSHIFT" => Operation.RShift,
                _ => throw new InvalidOperationException()
            };
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
    
    public void Execute(ref Dictionary<string, ushort> variables)
    {
        if (IsExecuted)
        {
            return;
        }
        
        var operand1Value = GetValue(_operand1, ref variables);
        var operand2Value = _operand2 == null ? null : GetValue(_operand2, ref variables);
        
        if (operand1Value == null)
        {
            return;
        }
        
        if (operand2Value == null && _operation != Operation.Not && _operation != Operation.Assign)
        {
            return;
        }
        
        var result = _operation switch
        {
            Operation.And => operand1Value & operand2Value,
            Operation.Or => operand1Value | operand2Value,
            Operation.Not => ~operand1Value,
            Operation.LShift => operand1Value << operand2Value,
            Operation.RShift => operand1Value >> operand2Value,
            Operation.Assign => operand1Value,
            _ => throw new InvalidOperationException()
        };
        
        if (result == null)
        {
            return;
        }
        
        variables[_destination] = (ushort)result;
        
        IsExecuted = true;
    }
    
    private static ushort? GetValue(string operand, ref Dictionary<string, ushort> variables)
    {
        if (ushort.TryParse(operand, out var value))
        {
            return value;
        }

        if (variables.TryGetValue(operand, out value))
        {
            return value;
        }

        return null;
    }
}