using aoc.helpers;

namespace aoc_2015_13;

internal class Program
{
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 13");
        
        var input = File.ReadAllLines("input.txt");
        
        var relationships = input.Select(ParseRelationship)
                                 .ToList();
        
        var people = relationships.Select(r => r.Person1)
                                  .Union(relationships.Select(r => r.Person2))
                                  .ToList();

        if (!isPart1)
        {
            foreach (var person in people)
            {
                relationships.Add(new Relationship("Me", person, 0));
                relationships.Add(new Relationship(person, "Me", 0));
            }
            
            people.Add("Me");
        }
        
        var permutations = Permutations.Permute(people.ToArray());
        
        var maxHappiness = permutations.Max(p => CalculateHappiness(p, relationships));
        
        Console.WriteLine($"Part {(isPart1 ? "1" : "2")}: {maxHappiness}");
    }

    private static Relationship ParseRelationship(string line)
    {
        var parts = line.Split(' ');

        var person1 = parts[0];
        var person2 = parts[^1].TrimEnd('.');
        var happiness = int.Parse(parts[3]) * (parts[2] == "gain" ? 1 : -1);

        return new Relationship(person1, person2, happiness);
    }
    
    private static int CalculateHappiness(IList<string> people, IList<Relationship> relationships)
    {
        var happiness = 0;
        
        for (var i = 0; i < people.Count; i++)
        {
            var person1 = people[i];
            var person2 = people[(i + 1) % people.Count];
            
            happiness += relationships.First(r => r.Person1 == person1 && r.Person2 == person2).Happiness;
            happiness += relationships.First(r => r.Person1 == person2 && r.Person2 == person1).Happiness;
        }

        return happiness;
    }
}

internal record Relationship(string Person1, string Person2, int Happiness);