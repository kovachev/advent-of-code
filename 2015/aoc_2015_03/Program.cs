namespace aoc_2015_03;

internal class Program
{
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 03");

        var santa = new Santa();
        var robot = new Santa();
        
        var input = File.ReadAllText("input.txt");
        
        var isSantaTurn = true;
        foreach (var directionCharacter in input.AsSpan())
        {
            if (isSantaTurn || isPart1)
            {
                santa.Move(directionCharacter);
            }
            else
            {
                robot.Move(directionCharacter);
            }
            
            isSantaTurn = !isSantaTurn;
        }
        
        var visitedHouses = santa.VisitedHousesCount();
        
        if (!isPart1)
        {
            visitedHouses += robot.VisitedHousesCount(santa.Visited);
        }
        
        Console.WriteLine($"Houses visited at least once: {visitedHouses}");
    }
}