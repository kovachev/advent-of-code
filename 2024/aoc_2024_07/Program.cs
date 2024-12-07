namespace aoc_2024_07;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 7");

        //var input = File.ReadAllLines("sample.txt");
        var input = File.ReadAllLines("input.txt");

        var equations = new List<Equation>();

        foreach (var line in input)
        {
            var split = line.Split(":");
            var result = long.Parse(split[0]);
            var values = split[1].Trim().Split(" ").Select(long.Parse).ToArray();
            var equation = new Equation(result, values);
            equations.Add(equation);
        }

        Console.WriteLine($"Part 1: {Solution(equations, part2: false)}");
        Console.WriteLine($"Part 2: {Solution(equations, part2: true)}");
    }

    private static long Solution(IEnumerable<Equation> equations, bool part2)
    {
        var sum = 0L;

        foreach (var equation in equations)
        {
            sum += SolveEquation(equation, part2);
        }

        return sum;
    }

    private static long SolveEquation(Equation equation, bool part2)
    {
        var values = equation.Values;
        var result = equation.Result;

        if (TrySolve(values, result, index: 0, values[0], part2, out var solution))
        {
            return solution;
        }

        return 0;
    }

    private static bool TrySolve(
        long[] values, 
        long result, 
        int index, 
        long current,
        bool part2,
        out long solution)
    {
        if (index == values.Length - 1)
        {
            if (current == result)
            {
                solution = current;
                return true;
            }

            solution = 0;
            return false;
        }

        if (TrySolve(values, result, index + 1, current + values[index + 1], part2, out solution) ||
            TrySolve(values, result, index + 1, current * values[index + 1], part2, out solution) ||
            (part2 && TrySolve(values, result, index + 1, long.Parse($"{current}{values[index+1]}"), part2, out solution)))
        {
            return true;
        }

        solution = 0;
        return false;
    }
}

internal record Equation(long Result, long[] Values);