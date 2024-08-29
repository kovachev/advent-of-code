namespace aoc_01;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 01");
        
        var input = File.ReadAllText("input.txt");
        
        var currentFloor = 0; // 0-based
        
        var positionInInput = 0;
        var isBasementReached = false;
        
        foreach (var directionCharacter in input.AsSpan())
        {
            positionInInput++; // 1-based
            
            switch (directionCharacter)
            {
                case '(':
                    currentFloor++;
                    break;
                
                case ')':
                    currentFloor--;
                    break;
            }

            if (currentFloor == -1 && !isBasementReached)
            {
                Console.WriteLine($"Input position when the basement is reached for the first time: {positionInInput}");
                isBasementReached = true;
            }
        }
        
        Console.WriteLine($"Final floor: {currentFloor}");
    }
}