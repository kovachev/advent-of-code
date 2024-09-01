namespace aoc_2015_25;

internal class Program
{
    internal static void Main(string[] args)
    {
        Console.WriteLine("Advent of Code 2015 - 25");
        
        // Part 1
        // To continue, please consult the code grid in the manual. Enter the code at row 2981, column 3075.
        const long row = 2981;
        const long column = 3075;
        const long startValue = 20151125;
        
        // Calculate column value at row 1
        var columnValueAtRow1 = 0;
        for (var i = 1; i <= column; i++)
        {
            columnValueAtRow1 += i;
        }
        
        Console.WriteLine("Column value at row 1: " + columnValueAtRow1);
        
        long iterations = columnValueAtRow1;
        var increment = column;
        for (var i = 0; i < row - 1; i++)
        {
            iterations += increment++;
        }

        iterations--; // We start at 1 as we already have the start value
        
        Console.WriteLine("Iterations needed to reach target value: " + iterations);
        
        var code = startValue;
        for (var i = 0; i < iterations; i++)
        {
            code = NextCode(code);
        }
        
        Console.WriteLine($"Part 1: The code at row {row}, column {column} is {code}");
    }
    
    private static long NextCode(long previousCode)
    {
        return (previousCode * 252533) % 33554393;
    }
}