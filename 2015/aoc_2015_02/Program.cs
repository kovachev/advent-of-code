namespace aoc_2015_02;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 02");
        
        var input = File.ReadAllLines("input.txt");
        
        var totalPaper = 0;
        var totalRibbon = 0;
        
        foreach (var line in input)
        {
            var box = new Box(line);
            totalPaper += box.SurfaceArea + box.SmallestSideArea;
            totalRibbon += box.SmallestCircumference + box.Volume;
        }
        
        Console.WriteLine($"Total paper needed: {totalPaper}");
        Console.WriteLine($"Total ribbon needed: {totalRibbon}");
    }
}