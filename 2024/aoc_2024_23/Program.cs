namespace aoc_2024_23;

using Nodes = Dictionary<string, HashSet<string>>;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 23");

        var nodes = Load("sample.txt");
        
        Part1(nodes);

        Console.WriteLine();
        Console.WriteLine();
        
        nodes = Load("input.txt");

        Part1(nodes);
    }

    private static void Part1(Nodes nodes)
    {
        var sequences = new List<string>();

        foreach (var node in nodes)
        {
            var sequencesOfTwo = GetNext(nodes, [node.Key]).ToArray();

            foreach (var sequenceOfTwo in sequencesOfTwo)
            {
                var sequencesOfThree = GetNext(nodes, sequenceOfTwo.ToArray()).Where(x => x.Any(y => y.StartsWith("t")))
                                                                              .Select(x => string.Join(",", x.Order()))
                                                                              .ToArray();

                foreach (var sequenceOfThree in sequencesOfThree)
                {
                    sequences.Add(sequenceOfThree);

                }
            }
        }

        sequences = sequences.Distinct()
                             .ToList();

        foreach (var sequence in sequences)
        {
            Console.WriteLine($"Sequence for {sequence[..2]}: {sequence}");
        }
        
        Console.WriteLine($"Total sequences: {sequences.Count}");
    }

    private static Nodes Load(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var nodes = new Nodes();

        foreach (var line in input)
        {
            var parts = line.Split("-");
            var node1 = parts[0];
            var node2 = parts[1];

            if (!nodes.ContainsKey(node1))
            {
                nodes[node1] = new HashSet<string>();
            }

            if (!nodes.ContainsKey(node2))
            {
                nodes[node2] = new HashSet<string>();
            }

            nodes[node1].Add(node2);
            nodes[node2].Add(node1);
        }

        Console.WriteLine($"Total nodes: {nodes.Count}");
        return nodes;
    }

    private static IEnumerable<IEnumerable<string>> GetNext(Nodes nodes, string[] sequence)
    {
        foreach (var neighbor in nodes[sequence.Last()])
        {
            if (sequence.Contains(neighbor))
            {
                continue;
            }
            
            if (sequence.Length > 1 && 
                !sequence.All(x => nodes[neighbor].Contains(x)))
            {
                continue;
            }
                
            yield return sequence.Append(neighbor);
        }
    }
}