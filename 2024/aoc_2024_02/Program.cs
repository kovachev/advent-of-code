namespace aoc_2024_02;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 2");
        
        var input = File.ReadAllLines("input.txt");
        
        Console.WriteLine($"Part 1: {input.Count(IsSafePart1)}");
        Console.WriteLine($"Part 2: {input.Count(IsSafePart2)}");
    }

    private static bool IsSafePart1(string line)
    {
        var ints = line.Split(' ')
                       .Select(int.Parse)
                       .ToArray();
        
        return Validate(ints).IsSafe;
    }
    
    private static bool IsSafePart2(string line)
    {
        var ints = line.Split(' ')
                       .Select(int.Parse)
                       .ToArray();

        var (isSafe, index) = Validate(ints);
        if (!isSafe)
        {
            var ints2 = ints.Take(index).Concat(ints.Skip(index + 1)).ToArray();
            (isSafe, _) = Validate(ints2);
            if (!isSafe)
            {
                ints2 = ints.Take(index + 1).Concat(ints.Skip(index + 2)).ToArray();
                (isSafe, _) = Validate(ints2);
                if (!isSafe)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // private static bool IsSafeBruteForce(string line)
    // {
    //     var ints = line.Split(' ')
    //                    .Select(int.Parse)
    //                    .ToArray();
    //     
    //     var (isSafe, _) = Validate(ints);
    //     if (!isSafe)
    //     {
    //         for (var i = 0; i < ints.Length; i++)
    //         {
    //             var ints2 = ints.Take(i).Concat(ints.Skip(i + 1)).ToArray();
    //             (isSafe, _) = Validate(ints2);
    //             if (isSafe)
    //             {
    //                 return true;
    //             }
    //         }
    //     }
    //
    //     return isSafe;
    // }

    private static (bool IsSafe, int Index) Validate(int[] input)
    {
        if (input.Length == 1)
        {
            return (true, 0);
        }
        
        var isAscending = input.Last() > input.First();
        
        for (var i = 0; i < input.Length - 1; i++)
        {
            var diff = Math.Abs(input[i] - input[i + 1]); 
            if (diff == 0 || diff > 3)
            {
                return (false, i);
            }
            
            if (isAscending && input[i] > input[i + 1])
            {
                return (false, i);
            }
            
            if (!isAscending && input[i] < input[i + 1])
            {
                return (false, i);
            }
        }

        return (true, 0);
    }
    
}