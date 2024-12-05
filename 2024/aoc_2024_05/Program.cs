namespace aoc_2024_05;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 5");

        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample.txt");

        var rulesList = new List<Rule>();
        var pagesList = new List<int[]>();
        
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            if (line.Contains('|'))
            {
                // rule
                var split = line.Split('|');
                rulesList.Add(new Rule(int.Parse(split.First()), int.Parse(split.Last())));
            }
            else
            {
                // pages
                var split = line.Split(',');
                pagesList.Add(split.Select(int.Parse).ToArray());
            }
        }

        var rules = rulesList.ToArray();
        
        var sumValid = 0;
        var sumInvalid = 0;
        foreach (var pages in pagesList)
        {
            if (IsValid(rules, pages))
            {
                var value = pages[pages.Length / 2];
                sumValid += value;
            }
            else
            {
                sumInvalid += FixOrder(rules, pages);
            }
        }
        
        Console.WriteLine($"Part 1: {sumValid}");
        Console.WriteLine($"Part 2: {sumInvalid}");
    }

    private static bool IsValid(Rule[] rulesList, int[] pages)
    {
        for (var i = 1; i < pages.Length; i++)
        {
            var rules = rulesList.Where(x => x.Page1 == pages[i]).Select(x => x.Page2).ToArray();
            if (pages.Take(i).Any(x => rules.Contains(x)))
            {
                return false;
            }
        }

        return true;
    }
    
    private static int FixOrder(Rule[] rulesList, int[] pages)
    {
        bool hasSwap;
        
        do
        {
            hasSwap = false;
            for (var i = 1; i < pages.Length; i++)
            {
                var rules = rulesList.Where(x => x.Page1 == pages[i]).ToArray();

                var violatedRules = rules.Where(x => pages.Take(i).Contains(x.Page2)).ToArray();

                foreach (var rule in violatedRules)
                {
                    var ip1 = Array.IndexOf(pages, rule.Page1);
                    var ip2 = Array.IndexOf(pages, rule.Page2);

                    pages[ip1] ^= pages[ip2];
                    pages[ip2] = pages[ip1] ^ pages[ip2];
                    pages[ip1] ^= pages[ip2];
                    hasSwap = true;
                }
            }
        } while (hasSwap);
        
        var value = pages[pages.Length / 2];
        
        return value;
    }
}

internal record Rule(int Page1, int Page2);