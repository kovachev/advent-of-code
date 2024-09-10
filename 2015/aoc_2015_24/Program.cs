namespace aoc_2015_24;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 24");
        
        var input = File.ReadAllLines("input.txt")
                        .Select(int.Parse)
                        .ToArray();

        //input = [1,2,3,4,5,7,8,9,10,11];
        
        var totalWeight = input.Sum();
        var groupWeight = totalWeight / 4;
        
        Console.WriteLine($"Total weight: {totalWeight}, group weight: {groupWeight}");

        for (var i = 1; i < input.Length; i++)
        {
            var g1 = Subsets(input, i, groupWeight).ToArray();
            if (g1.Length > 0)
            {
                Console.WriteLine("Length: " + i);
                var minQuantumEntanglement = g1.Min(g => g.Aggregate(1L, (a, b) => a * b));
                Console.WriteLine("Quantum Entanglement: " + minQuantumEntanglement);
                break;
            }
        }
    }
    
    public static IEnumerable<int[]> Subsets(int[]? objects, int maxLength, int weight)
    {
        if (objects == null || maxLength <= 0)
        {
            yield break;
        }

        var stack = new Stack<int>(maxLength);

        var i = 0;
        while (stack.Count > 0 || i < objects.Length)
        {
            if (i < objects.Length)
            {
                if (stack.Count == maxLength)
                {
                    i = stack.Pop() + 1;
                }

                stack.Push(i++);

                var subset = (from index in stack.Reverse() select objects[index]).ToArray();
                if (subset.Sum() == weight)
                {
                    yield return subset;
                }
            }
            else
            {
                i = stack.Pop() + 1;
                if (stack.Count > 0)
                {
                    i = stack.Pop() + 1;
                }
            }
        }
    }
}

internal class Solution
{
    public int[] Group1 { get; set; }
    
    public int[] Group2 { get; set; }
    
    public int[] Group3 { get; set; }

    public int Group1Length => Group1.Length;
    
    public int Group1QuantumEntanglement => Group1.Aggregate(1, (a, b) => a * b);
    
    public Solution(int[] group1, int[] group2, int[] group3)
    {
        Group1 = group1;
        Group2 = group2;
        Group3 = group3;
    }
    
    public bool IsValid()
    {
        return Group1.Sum() == Group2.Sum() && Group2.Sum() == Group3.Sum();
    }
}