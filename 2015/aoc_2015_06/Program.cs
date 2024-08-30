namespace aoc_2015_06;

internal class Program
{
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 06");
        
        const int gridWidth = 1000;
        const int gridHeight = 1000;
        
        var lightsGrid = new byte[gridWidth, gridHeight];
        
        for (var x = 0; x < gridWidth; x++)
        {
            for (var y = 0; y < gridHeight; y++)
            {
                lightsGrid[x, y] = 0;
            }
        }
        
        var input = File.ReadAllLines("input.txt");

        foreach (var line in input)
        {
            var instruction = new Instruction(line, isPart1);
            instruction.Apply(lightsGrid);
        }
        
        var lightsOn = 0;
        var totalBrightness = 0;
        
        for (var x = 0; x < gridWidth; x++)
        {
            for (var y = 0; y < gridHeight; y++)
            {
                if (lightsGrid[x, y] != 0)
                {
                    lightsOn++;
                }
                
                totalBrightness += lightsGrid[x, y];
            }
        }
        
        Console.WriteLine($"Lights on: {lightsOn}");
        Console.WriteLine($"Total brightness: {totalBrightness}");
    }
}

internal class Instruction
{
    private readonly uint _startX;
    private readonly uint _startY;
    private readonly uint _endX;
    private readonly uint _endY;

    private readonly Func<byte, byte> _transformation = b => b;
    
    internal Instruction(string instruction, bool isPart1)
    {
        var parts = instruction.Split(' ');
        
        switch (parts[0])
        {
            case "turn":
                switch (parts[1])
                {
                    case "on":
                        _transformation = isPart1 ? _ => 1 : b => (byte)(b + 1);
                        break;
                    
                    case "off":
                        _transformation = isPart1 ? _ => 0 : b => (byte)Math.Max(0, b - 1);
                        break;
                }
                break;
            
            case "toggle":
                _transformation = isPart1 ? b => (byte)(1 - b) : b => (byte)(b + 2);
                break;
        }
        
        var startCoordinates = parts[^3].Split(',');
        _startX = uint.Parse(startCoordinates[0]);
        _startY = uint.Parse(startCoordinates[1]);
        
        var endCoordinates = parts[^1].Split(',');
        _endX = uint.Parse(endCoordinates[0]);
        _endY = uint.Parse(endCoordinates[1]);
    }
    
    internal void Apply(byte[,] grid)
    {
        for (var x = _startX; x <= _endX; x++)
        {
            for (var y = _startY; y <= _endY; y++)
            {
                grid[x, y] = _transformation(grid[x, y]);
            }
        }
    }
}