using System.Text;

namespace aoc_2015_15;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 19");
        
        var input = File.ReadAllLines("input.txt");
        
        var replacements = new List<Replacement>();
        var molecule = string.Empty;
        
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            if (line.Contains("=>"))
            {
                replacements.Add(ParseReplacement(line));
            }
            else if (line.Length > 0)
            {
                molecule = line;
            }
        }   
        
        Part1(molecule, replacements);
        
        // Reverse the replacements for part 2
        replacements = replacements.Select(r => new Replacement(r.To, r.From))
                                   .ToList();
        
        Part2(molecule, replacements);
    }

    private static void Part1(string molecule, IEnumerable<Replacement> replacements)
    {
        var molecules = new HashSet<string>();

        foreach (var replacement in replacements)
        {
            Console.WriteLine($"{replacement.From} => {replacement.To}");

            var indexes = GetPossibleReplacements(molecule, replacement);

            foreach (var index in indexes)
            {
                molecules.Add(Replace(molecule, index, replacement));
            }
        }

        Console.WriteLine($"Part 1 (distinct molecules): {molecules.Count}");
    }

    private static void Part2(string molecule, IEnumerable<Replacement> replacements)
    {
        var random = new Random();
        
        var steps = 0;
        var m = molecule;
        while (m != "e")
        {
            var instructions = GetPossibleReplacements(m, replacements);
            
            if (!instructions.Any())
            {
                m = molecule;
                steps = 0;
                continue;
            }
            
            var instruction = instructions.ElementAt(random.Next(instructions.Count()));
            m = Replace(m, instruction.Index, instruction.Length, instruction.Replacement);
            steps++;
        }
        
        Console.WriteLine($"Part 2 (steps): {steps}");
    }

    private static string Replace(string molecule, int index, Replacement replacement)
    {
        var (from, to) = replacement.ToTuple();

        return Replace(molecule, index, from.Length, to);
    }

    private static string Replace(string molecule, int index, int fromLength, string replacement)
    {
        var sb = new StringBuilder(molecule);
        
        sb.Remove(index, fromLength);
        sb.Insert(index, replacement);
        
        return sb.ToString();
    }
    
    private static Replacement ParseReplacement(string line)
    {
        var parts = line.Split(" => ");
        return new Replacement(parts[0].Trim(), parts[1].Trim());
    }

    private static IOrderedEnumerable<int> GetPossibleReplacements(string molecule, Replacement replacement)
    {
        var index = 0;
        var indexes = new List<int>();
     
        var iterations = 0;
        while ((index = molecule.IndexOf(replacement.From, iterations == 0 ? index : index + 1, StringComparison.Ordinal)) != -1)
        {
            indexes.Add(index);
            iterations++;
        }
        
        return indexes.OrderDescending();
    }

    private static IEnumerable<ReplacementInstruction> GetPossibleReplacements(string molecule, IEnumerable<Replacement> replacements)
    {
        var result = new List<ReplacementInstruction>();

        foreach (var replacement in replacements)
        {
            var indexes = GetPossibleReplacements(molecule, replacement);
            
            foreach (var index in indexes)
            {
                result.Add(new ReplacementInstruction(index, replacement.From.Length, replacement.To));
            }
        }

        return result;
    }
}

internal record Replacement(string From, string To)
{
    public (string, string) ToTuple() => (From, To);
}

internal record ReplacementInstruction(int Index, int Length, string Replacement);