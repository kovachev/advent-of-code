namespace aoc_2015_15;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 15");

        var input = File.ReadAllLines("input.txt");

        var ingredients = input.Select(ParseIngredient)
                               .ToArray();

        var maxScore = 0;
        var maxScoreWithCalories = 0;

        var ingredientCount = ingredients.Length;
        var ingredientsConcentration = new byte[ingredientCount];

        for (var i = 0; i < ingredientCount; i++)
        {
            ingredientsConcentration[i] = 0;
        }

        foreach (var combination in Combinations(ingredientsConcentration))
        {
            //Console.WriteLine($"[{string.Join(", ", combination)}]");

            var capacity = 0;
            var durability = 0;
            var flavor = 0;
            var texture = 0;
            var calories = 0;

            for (var i = 0; i < ingredientCount; i++)
            {
                capacity += combination[i] * ingredients[i].Capacity;
                durability += combination[i] * ingredients[i].Durability;
                flavor += combination[i] * ingredients[i].Flavor;
                texture += combination[i] * ingredients[i].Texture;
                calories += combination[i] * ingredients[i].Calories;
            }

            if (capacity <= 0 || durability <= 0 || flavor <= 0 || texture <= 0)
            {
                continue;
            }

            var score = capacity * durability * flavor * texture;
            maxScore = Math.Max(maxScore, score);

            if (calories == 500)
            {
                maxScoreWithCalories = Math.Max(maxScoreWithCalories, score);
            }
        }

        Console.WriteLine($"Part 1: {maxScore}");
        Console.WriteLine($"Part 2: {maxScoreWithCalories}");
    }
    
    private static IEnumerable<byte[]> Combinations(byte[] input, int index = 0)
    {
        for (var i = 0; i <= 100; i++)
        {
            input[index] = (byte)i;

            var sum = Sum(input);
            if (sum == 100)
            {
                yield return input;
            }
            
            if (index < input.Length - 1)
            {
                foreach (var combination in Combinations(input, index + 1))
                {
                    sum = Sum(combination);
                    if (sum == 100)
                    {
                        yield return combination;
                    }
                }
            }
        }
    }
    
    // Using LINQ to sum the array is slower and allocates too much memory.
    private static ushort Sum(byte[] input)
    {
        ushort sum = 0;
        
        foreach (var v in input)
        {
            sum += v;
        }
        
        return sum;
    }
    
    private static Ingredient ParseIngredient(string line)
    {
        // Sprinkles: capacity 5, durability -1, flavor 0, texture 0, calories 5
        // 0          1        2  3          4   5      6  7       8  9        10
        
        var parts = line.Split(' ');
        
        var capacity = int.Parse(parts[2].TrimEnd(','));
        var durability = int.Parse(parts[4].TrimEnd(','));
        var flavor = int.Parse(parts[6].TrimEnd(','));
        var texture = int.Parse(parts[8].TrimEnd(','));
        var calories = int.Parse(parts[10]);
        
        return new Ingredient(capacity, durability, flavor, texture, calories);
    }
}

internal record Ingredient(int Capacity, int Durability, int Flavor, int Texture, int Calories);