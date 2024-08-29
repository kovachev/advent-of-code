namespace aoc_2015_03;

internal class Program
{
    internal static void Main(bool isPart1)
    {
        Console.WriteLine("Advent of Code 2015 - 03");
        
        var visited = new Dictionary<string, uint>();
        
        var input = File.ReadAllText("input.txt");
        
        var santaX = 0;
        var santaY = 0;
        
        var robotX = 0;
        var robotY = 0;
        
        visited.Add($"{santaX},{santaY}", isPart1 ? 1u : 2u);
        
        var isSantaTurn = true;
        foreach (var directionCharacter in input.AsSpan())
        {
            var dX = 0;
            var dY = 0;
            
            switch (directionCharacter)
            {
                case '^':
                    dY = 1;
                    break;
                
                case 'v':
                    dY = -1;
                    break;
                
                case '>':
                    dX = 1;
                    break;
                
                case '<':
                    dX = -1;
                    break;
            }
            
            string key;
            
            if (isSantaTurn || isPart1)
            {
                santaX += dX;
                santaY += dY;
                
                key = $"{santaX},{santaY}";
                
                //Console.WriteLine($"Santa at {santaX},{santaY}");
            }
            else
            {
                robotX += dX;
                robotY += dY;
                
                key = $"{robotX},{robotY}";
                
                //Console.WriteLine($"Robot at {robotX},{robotY}");
            }
            
            isSantaTurn = !isSantaTurn;
            
            if (!visited.TryAdd(key, 1))
            {
                visited[key]++;
            }
        }
        
        Console.WriteLine($"Houses visited at least once: {visited.Count}");
    }
}