namespace aoc_2015_15;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 16");

        var input = File.ReadAllLines("input.txt");

        var auntSues = input.Select(ParseAuntSue)
                            .ToList();

        var knownProperties = new Dictionary<string, int>
        {
            ["children"] = 3,
            ["cats"] = 7,
            ["samoyeds"] = 2,
            ["pomeranians"] = 3,
            ["akitas"] = 0,
            ["vizslas"] = 0,
            ["goldfish"] = 5,
            ["trees"] = 3,
            ["cars"] = 2,
            ["perfumes"] = 1
        };

        var matches = auntSues.Where(sue => sue.Properties.All(property => knownProperties[property.Key] == property.Value));

        Console.WriteLine($"Part 1: {matches.Single().Number}");

        matches = auntSues.Where(sue => sue.Properties
                                           .All(property => property.Key switch
                                           {
                                               "cats" or "trees" => knownProperties[property.Key] < property.Value,
                                               "pomeranians" or "goldfish" => knownProperties[property.Key] > property.Value,
                                               _ => knownProperties[property.Key] == property.Value
                                           }));
        
        Console.WriteLine($"Part 2: {matches.Single().Number}");
    }

    private static AuntSue ParseAuntSue(string line)
    {
        var index = line.IndexOf(':');
        var number = int.Parse(line.Substring(4, index - 4));

        var parts = line[(index + 2)..].Split(", ");

        var properties = new Dictionary<string, int>();
        foreach (var part in parts)
        {
            var keyValue = part.Split(": ");
            properties[keyValue[0]] = int.Parse(keyValue[1]);
        }

        return new AuntSue(number, properties);
    }
}

internal record AuntSue(int Number, Dictionary<string, int> Properties);