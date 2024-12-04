namespace aoc_2024_04;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 4");
        
        var input = File.ReadAllLines("input.txt");
        
        // XMAS

        var part1count = 0;
        
        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input.Length; col++)
            {
                if (input[row][col] == 'X')
                {
                    var xPosition = new Position(row, col);
                    
                    var mNeighbours = GetNeighbours(input, xPosition, 'M');

                    foreach (var mPosition in mNeighbours)
                    {
                        var aNeighbours = GetNeighbours(input, mPosition, 'A');

                        foreach (var aPosition in aNeighbours)
                        {
                            var sNeighbours = GetNeighbours(input, aPosition, 'S');

                            foreach (var sPosition in sNeighbours)
                            {
                                Position[] positions = [xPosition, mPosition, aPosition, sPosition];
                                
                                if (IsValid(positions))
                                {
                                    part1count++;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        Console.WriteLine($"Part 1: {part1count}");
    }
    
    private static IEnumerable<Position> GetNeighbours(string[] input, Position position, char letter)
    {
        var neighbours = new List<Position>();
        
        foreach (var neighbour in GetNeighbours(position, input.Length, input[0].Length))
        {
            if (input[neighbour.Row][neighbour.Col] == letter)
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }
    
    private static IEnumerable<Position> GetNeighbours(Position position, int rows, int cols)
    {
        for (var row = position.Row - 1; row <= position.Row + 1; row++)
        {
            for (var col = position.Col - 1; col <= position.Col + 1; col++)
            {
                if (row >= 0 && row < rows && 
                    col >= 0 && col < cols && 
                    (row != position.Row || col != position.Col))
                {
                    yield return new Position(row, col);
                }
            }
        }
    }

    private static bool IsValid(Position[] positions)
    {
        if (positions.All(x => x.Row == positions[0].Row) || // Same row
            positions.All(x => x.Col == positions[0].Col) || // Same column
            positions.All(x => x.Row - x.Col == positions[0].Row - positions[0].Col) || // Same diagonal (top-left to bottom-right)
            positions.All(x => x.Row + x.Col == positions[0].Row + positions[0].Col)) // Same diagonal (top-right to bottom-left)
        {
            return true;
        }
        
        return false;
    }
}

internal record Position(int Row, int Col);