namespace aoc_2015_05;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 05");
        
        var input = File.ReadAllLines("input.txt");
        
        var niceCountForRules1 = 0;
        var niceCountForRules2 = 0;
        
        foreach (var line in input)
        {
            var lineSpan = line.AsSpan();
            
            if (IsNiceBasedOnFirstRuleSet(lineSpan))
            {
                niceCountForRules1++;
            }
            
            if (IsNiceBasedOnSecondRuleSet(lineSpan))
            {
                niceCountForRules2++;
            }
        }
        
        Console.WriteLine($"Nice strings for rules 1: {niceCountForRules1}");
        Console.WriteLine($"Nice strings for rules 2: {niceCountForRules2}");
    }
    
    private static readonly string[] ForbiddenCombinations = ["ab", "cd", "pq", "xy"];
    
    private static bool IsNiceBasedOnFirstRuleSet(ReadOnlySpan<char> line)
    {
        var vowelsCount = 0;
        var hasDoubleLetter = false;

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] is 'a' or 'e' or 'i' or 'o' or 'u')
            {
                vowelsCount++;
            }

            if (i > 0 && line[i] == line[i - 1])
            {
                hasDoubleLetter = true;
            }

            if (i > 0 && ForbiddenCombinations.Contains($"{line[i-1]}{line[i]}"))
            {
                return false;
            }
        }

        return vowelsCount >= 3 && hasDoubleLetter;
    }
    
    private static bool IsNiceBasedOnSecondRuleSet(ReadOnlySpan<char> line)
    {
        var hasPairOfLettersTwice = false;
        var hasLetterThatRepeatsWithOneLetterBetween = false;
        
        for (var i = 0; i < line.Length - 1; i++)
        {
            var pair = $"{line[i]}{line[i + 1]}";
            
            var offset = i + 2;
            
            if (line[offset..].IndexOf(pair) != -1)
            {
                hasPairOfLettersTwice = true;
            }
            
            if (i < line.Length - 2 && line[i] == line[i + 2])
            {
                hasLetterThatRepeatsWithOneLetterBetween = true;
            }
        }
        
        return hasPairOfLettersTwice && hasLetterThatRepeatsWithOneLetterBetween;
    }
}