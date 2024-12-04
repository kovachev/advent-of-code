namespace aoc_2024_04;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 4");
        
        var sample = File.ReadAllLines("sample.txt");
        
        Console.WriteLine($"Part 1 (sample): {Part1(sample)}");
        Console.WriteLine($"Part 2 (sample): {Part2(sample)}");
        
        var input = File.ReadAllLines("input.txt");
        
        Console.WriteLine($"Part 1: {Part1(input)}");
        Console.WriteLine($"Part 2: {Part2(input)}");
    }

    private static int Part1(string[] input)
    {
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

        return part1count;
    }

    private static int Part2(string[] input)
    {
        // X-MAS
        var part2count = 0;

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input.Length; col++)
            {
                if (input[row][col] == 'A')
                {
                    var aPosition = new Position(row, col);

                    var leftTopPosition = new Position(aPosition.Row - 1, aPosition.Col - 1);
                    var rightBottomPosition = new Position(aPosition.Row + 1, aPosition.Col + 1);
                    var rightTopPosition = new Position(aPosition.Row - 1, aPosition.Col + 1);
                    var leftBottomPosition = new Position(aPosition.Row + 1, aPosition.Col - 1);
                    
                    if (!IsValid(input, leftTopPosition) ||
                        !IsValid(input, rightBottomPosition) ||
                        !IsValid(input, rightTopPosition) ||
                        !IsValid(input, leftBottomPosition))
                    {
                        continue;
                    }
                    
                    var leftTopLetter = input[leftTopPosition.Row][leftTopPosition.Col];
                    var rightBottomLetter = input[rightBottomPosition.Row][rightBottomPosition.Col];
                    var rightTopLetter = input[rightTopPosition.Row][rightTopPosition.Col];
                    var leftBottomLetter = input[leftBottomPosition.Row][leftBottomPosition.Col];
                    
                    if ((leftTopLetter == 'M' && rightTopLetter == 'M' && 
                        rightBottomLetter == 'S'  && leftBottomLetter == 'S') ||
                        
                        (leftTopLetter == 'S' && rightTopLetter == 'S' && 
                         rightBottomLetter == 'M'  && leftBottomLetter == 'M') ||
                        
                        (leftTopLetter == 'S' && rightTopLetter == 'M' && 
                         rightBottomLetter == 'S'  && leftBottomLetter == 'M') ||
                        
                        (leftTopLetter == 'M' && rightTopLetter == 'S' && 
                         rightBottomLetter == 'M'  && leftBottomLetter == 'S'))
                    {
                        part2count++;
                    }
                }
            }
        }

        return part2count;
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
    
    private static bool IsValid(string[] input, Position position)
    {
        var rows = input.Length;
        var cols = input[0].Length;

        if (position.Row >= 0 && position.Row < rows &&
            position.Col >= 0 && position.Col < cols)
        {
            return true;
        }

        return false;
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