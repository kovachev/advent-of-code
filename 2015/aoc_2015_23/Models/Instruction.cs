namespace aoc_2015_23;

internal class Instruction
{
    public string Operation { get; }
    
    public string Register { get; }
    
    public int Offset { get; }
    
    public Instruction(string operation, string register, int offset)
    {
        Operation = operation;
        Register = register;
        Offset = offset;
    }
}