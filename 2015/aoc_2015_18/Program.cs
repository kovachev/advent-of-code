namespace aoc_2015_15;

internal class Program
{
    internal static void Main(bool isPart1 = false)
    {
        Console.WriteLine("Advent of Code 2015 - 18");
        Console.WriteLine("Conway's Game of Life");

        var input = File.ReadAllLines("input.txt");

        var grid = new byte[100, 100];
        
        var x = 0;
        var y = 0;
        foreach (var line in input)
        {
            foreach (var ch in line.AsSpan())
            {
                grid[x, y] = ch == '#' ? (byte)1 : (byte)0;
                x++;
            }
            y++;
            x = 0;
        }

        if (!isPart1)
        {
            grid = TurnOnCorners(grid);
        }
        
        for (var i = 0; i < 100; i++)
        {
            grid = NextGrid(grid);
            
            if (!isPart1)
            {
                grid = TurnOnCorners(grid);
            }
        }

        var count = 0;
        for (x = 0; x < grid.GetLength(0); x++)
        {
            for (y = 0; y < grid.GetLength(1); y++)
            {
                count += grid[x, y];
            }
        }
        
        Console.WriteLine(count);
    }
    
    private static byte[,] TurnOnCorners(byte[,] grid)
    {
        grid[0, 0] = 1;
        grid[0, grid.GetLength(1) - 1] = 1;
        grid[grid.GetLength(0) - 1, 0] = 1;
        grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1] = 1;
        
        return grid;
    }
    
    private static byte[,] NextGrid(byte[,] grid)
    {
        var next = CloneGrid(grid);
        
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var count = CountNeighbors(grid, x, y);
                
                if (grid[x, y] == 1)
                {
                    next[x, y] = count is 2 or 3 ? (byte)1 : (byte)0;
                }
                else
                {
                    next[x, y] = count is 3 ? (byte)1 : (byte)0;
                }
            }
        }
        return next;
    }
    
    private static int CountNeighbors(byte[,] grid, int x, int y)
    {
        var count = 0;
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }
                
                var nx = x + dx;
                var ny = y + dy;
                
                if (nx < 0 || nx >= grid.GetLength(0) || 
                    ny < 0 || ny >= grid.GetLength(1))
                {
                    continue;
                }
                
                count += grid[nx, ny];
            }
        }
        return count;
    }
    
    private static byte[,] CloneGrid(byte[,] grid)
    {
        var clone = new byte[grid.GetLength(0), grid.GetLength(1)];
        
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                clone[x, y] = grid[x, y];
            }
        }
        
        return clone;
    }
}