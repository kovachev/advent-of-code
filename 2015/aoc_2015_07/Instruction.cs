namespace aoc_2015_07;

internal class Instruction
{
    private readonly string _operand1;
    private readonly string? _operand2;
    private readonly string _destination;
    private readonly Operation _operation;
    
    private readonly PlugBoard _plugBoard;
    
    public bool IsExecuted { get; private set; }

    internal Instruction(string instruction, PlugBoard plugBoard)
    {
        _plugBoard = plugBoard ?? throw new ArgumentNullException(nameof(plugBoard));
        
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
    
    public void Execute()
    {
        if (IsExecuted)
        {
            return;
        }
        
        var operand1Value = _plugBoard.GetValue(_operand1);
        var operand2Value = _operand2 == null ? null : _plugBoard.GetValue(_operand2);
        
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
        
        _plugBoard.SetValue(_destination, (ushort)result);
        
        IsExecuted = true;
    }

    public bool CanExecute()
    {
        var operand1Value = _plugBoard.GetValue(_operand1);
        var operand2Value = _operand2 == null ? null : _plugBoard.GetValue(_operand2);
        
        if (operand1Value == null)
        {
            return false;
        }
        
        if (operand2Value == null && _operation != Operation.Not && _operation != Operation.Assign)
        {
            return false;
        }

        return true;
    }
}