using Helpers;

namespace aoc_2024_04;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 4");
        
        var sample = new Map("sample.txt");
        
        Console.WriteLine($"Part 1 (sample): {Part1(sample)}");
        Console.WriteLine($"Part 2 (sample): {Part2(sample)}");
        
        var input = new Map("input.txt");
        
        Console.WriteLine($"Part 1: {Part1(input)}");
        Console.WriteLine($"Part 2: {Part2(input)}");
    }

    private static int Part1(Map map)
    {
        // XMAS

        var count = 0;

        foreach (var (xPosition, _) in map.Where(x => x.Value == 'X'))
        {
            foreach (var mPosition in map.GetNeighbours(xPosition, c => c == 'M', includeDiagonals: true))
            {
                foreach (var aPosition in map.GetNeighbours(mPosition, c => c == 'A', includeDiagonals: true))
                {
                    foreach (var sPosition in map.GetNeighbours(aPosition, c => c == 'S', includeDiagonals: true))
                    {
                        var positions = new[] { xPosition, mPosition, aPosition, sPosition };

                        if (Position.AreOnSameRow(positions) ||
                            Position.AreOnSameColumn(positions) ||
                            Position.AreOnSameDiagonal(positions))
                        {
                            count++;
                        }
                    }
                }
            }
        }

        return count;
    }

    private static int Part2(Map map)
    {
        // X-MAS
        var count = 0;

        foreach (var (aPosition, _) in map.Where(x => x.Value == 'A'))
        {
            var leftTopPosition = aPosition + Position.UpLeft;
            var rightBottomPosition = aPosition + Position.DownRight;
            var rightTopPosition = aPosition + Position.UpRight;
            var leftBottomPosition = aPosition + Position.DownLeft;

            if (!map.IsOnMap(leftTopPosition) ||
                !map.IsOnMap(rightBottomPosition) ||
                !map.IsOnMap(rightTopPosition) ||
                !map.IsOnMap(leftBottomPosition))
            {
                continue;
            }

            var leftTopLetter = map[leftTopPosition];
            var rightBottomLetter = map[rightBottomPosition];
            var rightTopLetter = map[rightTopPosition];
            var leftBottomLetter = map[leftBottomPosition];

            if ((leftTopLetter == 'M' && rightTopLetter == 'M' &&
                 leftBottomLetter == 'S' && rightBottomLetter == 'S') ||

                (leftTopLetter == 'S' && rightTopLetter == 'S' &&
                 leftBottomLetter == 'M' && rightBottomLetter == 'M') ||

                (leftTopLetter == 'S' && rightTopLetter == 'M' &&
                 leftBottomLetter == 'S' && rightBottomLetter == 'M') ||

                (leftTopLetter == 'M' && rightTopLetter == 'S' &&
                 leftBottomLetter == 'M' && rightBottomLetter == 'S'))
            {
                count++;
            }
        }

        return count;
    }
}