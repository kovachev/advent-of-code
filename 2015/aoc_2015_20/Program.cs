namespace aoc_2015_15;

internal class Program
{
    // dotnet run -- --start-house 660000 --increment 1 --is-part1 false
    internal static void Main(
        long target = 29_000_000,
        long startHouse = 660_000,
        int increment = 1,
        bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 20");
        
        var house = startHouse;
        var presents = 0L;
        
        while (presents < target)
        {
            house += increment;
            presents = GetPresents(house, isPart1: isPart1);
            
            if (house % 1000 == 0)
            {
                Console.WriteLine($"House {house:N0} got {presents:N0} presents");
            }
        }
        
        Console.WriteLine($"Part 1: {house}");
    }
    
    private static long GetPresents(long house, bool isPart1)
    {
        var presents = 0L;
     
        for (var i = 1; i <= house; i++)
        {
            switch (isPart1)
            {
                case true when house % i == 0:
                    presents += i * 10;
                    break;
               
                case false when house % i == 0 && house / i <= 50:
                    presents += i * 11;
                    break;
            }
        }
        
        return presents;
    }
}