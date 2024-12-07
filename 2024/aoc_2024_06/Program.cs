namespace aoc_2024_06;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 6");
        
        var input = File.ReadAllLines("input.txt");
        //var input = File.ReadAllLines("sample.txt");

        var map = new char[input[0].Length, input.Length];
        
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                map[x,y] = input[y][x];
            }
        }
        
        var position = FindStartingPoint(map);
        map[position.X, position.Y] = 'X';
        
        var offset = -1;
        var horizontal = false;

        var len = 1;
        while (true)
        {
            var x = horizontal ? position.X + offset : position.X;
            var y = horizontal ? position.Y : position.Y + offset;
            
            var next = new Position(x, y);
            if (!IsOnMap(map, next))
            {
                break;
            }
                
            if (map[next.X, next.Y] == '#')
            {
                offset = horizontal == false ? -offset : offset;
                horizontal = !horizontal;
            }
            else
            {
                if (map[next.X, next.Y] == '.')
                {
                    map[next.X, next.Y] = 'X';
                    len++;
                }
                
                position = next;
            }
        }
        
        Console.WriteLine($"Part 1: {len}");
    }
    
    private static bool IsOnMap(char[,] map, Position position)
    {
        return position.X >= 0 && position.X < map.GetLength(0) &&
               position.Y >= 0 && position.Y < map.GetLength(1);
    }
    
    private static Position FindStartingPoint(char[,] map)
    {
        for (var y = 0; y < map.GetLength(1); y++)
        {
            for (var x = 0; x < map.GetLength(0); x++)
            {
                if (map[x,y] == '^')
                {
                    return new Position(x, y);
                }
            }
        }
        
        return new Position(-1, -1);
    }
}

internal record Position(int X, int Y);