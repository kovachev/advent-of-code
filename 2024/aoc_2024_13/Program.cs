using System.Text.RegularExpressions;

namespace aoc_2024_13;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 13");

        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample.txt");

        var buttonAExpression = new Regex(@"Button A: X\+(?<x>\d+), Y\+(?<y>\d+)", RegexOptions.Compiled);
        var buttonBExpression = new Regex(@"Button B: X\+(?<x>\d+), Y\+(?<y>\d+)", RegexOptions.Compiled);
        var prizeExpression = new Regex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)", RegexOptions.Compiled);

        var conditions = new List<ButtonsAndPrize>();
        
        Button buttonA = null;
        Button buttonB = null;
        Prize prize = null;
        foreach (var line in input)
        {
            if (buttonAExpression.IsMatch(line))
            {
                var match = buttonAExpression.Match(line);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                buttonA = new Button(x, y);
            }
            
            if (buttonBExpression.IsMatch(line))
            {
                var match = buttonBExpression.Match(line);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                buttonB = new Button(x, y);
            }
            
            if (prizeExpression.IsMatch(line))
            {
                var match = prizeExpression.Match(line);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                prize = new Prize(x, y);
                
                conditions.Add(new ButtonsAndPrize(buttonA, buttonB, prize));
            }
        }

        foreach (var condition in conditions)
        {
            Console.WriteLine($"Button A: {condition.ButtonA}");
            Console.WriteLine($"Button B: {condition.ButtonB}");
            Console.WriteLine($"Prize: {condition.Prize}");
            Console.WriteLine($"Price: {condition.Price()}");
            Console.WriteLine();
        }
        
        var part1 = conditions.Sum(c => c.Price());
        Console.WriteLine($"Sum: {part1}");
        
        var part2 = conditions.Sum(c => c.Price(10000000000000));
        Console.WriteLine($"Sum: {part2}");
    }
}

internal record Button(int X, int Y);

internal record Prize(int X, int Y);

internal record ButtonsAndPrize(Button ButtonA, Button ButtonB, Prize Prize)
{
    public long Price(double offset = 0)
    {
        // X = a * Xa + b * Xb
        // Y = a * Ya + b * Yb
        
        var (a, b) = GetCoefficients(offset);

        const double tolerance = 0.0000001;

        if (offset == 0)
        {
            if (a > 100 || 
                b > 100)
            {
                return 0;
            }
        }
        
        if (Math.Abs(a - Math.Floor(a)) > tolerance || 
            Math.Abs(b - Math.Floor(b)) > tolerance)
        {
            return 0;
        }
        
        return (long)a * 3 + (long)b * 1;
    }
    
    private (double a, double b) GetCoefficients(double offset = 0)
    {
        double xa = ButtonA.X;
        double ya = ButtonA.Y;
        double xb = ButtonB.X;
        double yb = ButtonB.Y;
        double x = Prize.X + offset;
        double y = Prize.Y + offset;

        var denominator = xa * yb - xb * ya;
        var a = (x * yb - xb * y) / denominator;
        var b = (xa * y - x * ya) / denominator;

        return (a, b);
    }
}