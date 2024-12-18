namespace aoc_2024_12;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 10");
        
        //var input = File.ReadAllLines("input.txt");
        var input = File.ReadAllLines("sample1.txt");
        //var input = File.ReadAllLines("sample2.txt");
        //var input = File.ReadAllLines("sample3.txt");

        var map = input.Select(x => x.ToCharArray()).ToArray();

        var regions = new List<Region>();

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map.Length; x++)
            {
                var position = new Position(x, y);
                if (!regions.Any(r => r.Contains(position)))
                {
                    var newRegion = Traverse(map, position);
                    regions.Add(newRegion);
                }
            }
        }
    }

    private static Region Traverse(char[][] map, Position start)
    {
        var region = new Region(map[start.Y][start.X], []);

        
        
        return region;
    }
}

internal record Region(char Type, List<Position> content)
{
    public bool Contains(Position position)
    {
        return content.Any(x => x.X == position.X && x.Y == position.Y);
    }
}; 

internal record Position(int X, int Y, Position Parent = null)
{
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    
    public bool IsOnMap(char[][] map) => X >= 0 && X < map[0].Length && Y >= 0 && Y < map.Length;

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}