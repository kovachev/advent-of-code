using System.Text.RegularExpressions;

namespace aoc_2024_03;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 3");
        
        var input = File.ReadAllText("input.txt");

        var sum = 0;
        
        var matches = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)").Matches(input);
        
        var doIndexes = new Regex(@"do\(\)").Matches(input)
                                            .Select(x => x.Index)
                                            .ToArray();
        
        var dontIndexes = new Regex(@"don't\(\)").Matches(input)
                                                 .Select(x => x.Index)
                                                 .ToArray();
        
        var enabledRanges = ComputeRanges(doIndexes, dontIndexes, input.Length);

        foreach (Match match in matches)
        {
            if (enabledRanges.Any(x => x.Start < match.Index && match.Index < x.End))
            {
                sum += GetValue(match);    
            }
        }
        
        Console.WriteLine($"Part 1: {matches.Sum(GetValue)}");
        Console.WriteLine($"Part 2: {sum}");
    }

    private static int GetValue(Match match)
    {
        var a = int.Parse(match.Groups[1].Value);
        var b = int.Parse(match.Groups[2].Value);
        
        return a * b;
    }
    
    private static IEnumerable<Range> ComputeRanges(int[] doIndexes, int[] dontIndexes, int inputLength)
    {
        var enabledRanges = new List<Range> { new (0, dontIndexes.First()) };

        var prevDontIndex = dontIndexes.First();
        foreach (var currDontIndex in dontIndexes.Skip(1))
        {
            var previousDoIndex = doIndexes.FirstOrDefault(x => x > prevDontIndex && x < currDontIndex);
            
            prevDontIndex = currDontIndex;
            
            if (previousDoIndex == 0)
            {
                continue;
            }
            
            enabledRanges.Add(new Range(previousDoIndex, currDontIndex));
        }

        if (doIndexes.Last() > dontIndexes.Last())
        {
            enabledRanges.Add(new Range(doIndexes.Last(), inputLength));
        }

        return enabledRanges;
    }
}

internal record Range(int Start, int End);