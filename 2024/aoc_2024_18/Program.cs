using Helpers;

namespace aoc_2024_18;

internal class Program
{
    private const char WallMarker = '#';

    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 18");

        Part1("sample.txt", new Map(7, 7), 12);

        Part1("input.txt", new Map(71, 71), 1024);

        Part2("sample.txt", new Map(7, 7), 12);

        Part2("input.txt", new Map(71, 71), 1024);
    }

    private static void Part1(string inputFile, Map map, int takeSize)
    {
        var startPosition = new Position(0, 0);
        var endPosition = new Position(map.XMax - 1, map.YMax - 1);

        var input = File.ReadAllLines(inputFile);
        AddWallsToMap(input, map, takeSize);

        Console.WriteLine($"Looking for path from {startPosition} [{map[startPosition]}] to {endPosition} [{map[endPosition]}].");

        var path = map.FindPath(startPosition, endPosition, debug: false);

        Console.WriteLine($"Part 1 {inputFile}: {path?.Score}");
    }

    private static void Part2(string inputFile, Map map, int takeSize)
    {
        var startPosition = new Position(0, 0);
        var endPosition = new Position(map.XMax - 1, map.YMax - 1);

        var input = File.ReadAllLines(inputFile);

        var (lo, hi) = (takeSize, input.Length);
        while (hi - lo > 1)
        {
            var m = (lo + hi) / 2;
            AddWallsToMap(input, map, m);
            if (map.FindPath(startPosition, endPosition) == null)
            {
                hi = m;
            }
            else
            {
                lo = m;
            }
        }

        Console.WriteLine($"Part 2 {inputFile}: {input[lo]}");
    }

    private static void AddWallsToMap(string[] lines, Map map, int takeSize)
    {
        map.Clear();
        foreach (var line in lines.Take(takeSize))
        {
            var parts = line.Split(',');
            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);
            map[x, y] = WallMarker;
        }
    }
}