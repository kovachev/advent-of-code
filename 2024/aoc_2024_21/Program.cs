using System.Collections.Concurrent;
using System.Text;
using Helpers;

namespace aoc_2024_21;

public class Program
{
    private static readonly ConcurrentDictionary<(char, char, int), long> _cache = new();

    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 21");

        _cache.Clear();

        Process("Part 1", "sample.txt", 2);

        Console.WriteLine();
        Console.WriteLine();

        Process("Part 1", "input.txt", 2);

        Console.WriteLine();
        Console.WriteLine();

        _cache.Clear();

        Process("Part 2", "sample.txt", 25);

        Console.WriteLine();
        Console.WriteLine();

        Process("Part 2", "input.txt", 25);
    }

    private static void Process(string prefix, string inputFile, int depth)
    {
        var lines = File.ReadAllLines(inputFile);

        var result = 0L;

        var pads = Enumerable.Repeat(GetDirectionalPad(), depth)
                             .Prepend(GetNumericPad())
                             .ToArray();

        foreach (var line in lines)
        {
            var length = ConvertSequence(pads, line);

            var multiplier = int.Parse(line[..^1]);
            var complexity = length * multiplier;

            result += complexity;

            Console.WriteLine();
            Console.WriteLine($"{line} Complexity: {length} * {multiplier} = {complexity}");
            Console.WriteLine();
            Console.WriteLine();
        }

        Console.WriteLine($"{prefix} [{inputFile}] Total Complexity: {result}");
    }

    private static long ConvertSequence(KeyPad[] pads, string input)
    {
        if (pads.Length == 0)
        {
            return input.Length;
        }

        var fromKey = 'A';
        var length = 0L;

        foreach (var toKey in input.AsSpan())
        {
            length += ConvertKey(fromKey, toKey, pads);

            fromKey = toKey;
        }

        return length;
    }

    private static long ConvertKey(char fromKey, char toKey, KeyPad[] pads)
    {
        return _cache.GetOrAdd((fromKey, toKey, pads.Length), _ =>
        {
            var targetPad = pads[0];

            var fromPosition = targetPad.Keys[fromKey];
            var toPosition = targetPad.Keys[toKey];
            var forbiddenPosition = targetPad.Keys[' '];

            var dy = toPosition.Y - fromPosition.Y;
            var dx = toPosition.X - fromPosition.X;

            var y = dy > 0 ? 'v' : '^';
            var x = dx > 0 ? '>' : '<';

            var horizontal = new string(x, Math.Abs(dx));
            var vertical = new string(y, Math.Abs(dy));

            var length = long.MaxValue;

            if (forbiddenPosition != new Position(toPosition.X, fromPosition.Y))
            {
                length = Math.Min(length, ConvertSequence(pads[1..], $"{horizontal}{vertical}A"));
            }

            if (forbiddenPosition != new Position(fromPosition.X, toPosition.Y))
            {
                length = Math.Min(length, ConvertSequence(pads[1..], $"{vertical}{horizontal}A"));
            }

            return length;
        });
    }

    private static string ExecuteOnNumericPad(string input)
    {
        var numericPad = GetNumericPad();
        var padPosition = numericPad.Keys['A'];

        var result = new StringBuilder();

        foreach (var ch in input.AsSpan())
        {
            if (ch == 'A')
            {
                result.Append(numericPad.Keys.Single(x => x.Value == padPosition).Key);
                continue;
            }

            var direction = ch switch
            {
                '^' => Position.Up,
                'v' => Position.Down,
                '<' => Position.Left,
                '>' => Position.Right,
                _ => throw new ArgumentOutOfRangeException()
            };

            padPosition += direction;
        }

        return result.ToString();
    }

    private static string ExecuteOnDirectionalPad(string input)
    {
        var directionalPad = GetDirectionalPad();
        var padPosition = directionalPad.Keys['A'];

        var result = new StringBuilder();

        foreach (var ch in input.AsSpan())
        {
            if (ch == 'A')
            {
                result.Append(directionalPad.Keys.Single(x => x.Value == padPosition).Key);
                continue;
            }

            var direction = ch switch
            {
                '^' => Position.Up,
                'v' => Position.Down,
                '<' => Position.Left,
                '>' => Position.Right,
                _ => throw new ArgumentOutOfRangeException()
            };

            padPosition += direction;
        }

        return result.ToString();
    }

    private static KeyPad GetNumericPad()
    {
        var keys = new Dictionary<char, Position>();

        keys['7'] = new Position(0, 0);
        keys['8'] = new Position(1, 0);
        keys['9'] = new Position(2, 0);

        keys['4'] = new Position(0, 1);
        keys['5'] = new Position(1, 1);
        keys['6'] = new Position(2, 1);

        keys['1'] = new Position(0, 2);
        keys['2'] = new Position(1, 2);
        keys['3'] = new Position(2, 2);

        keys[' '] = new Position(0, 3);
        keys['0'] = new Position(1, 3);
        keys['A'] = new Position(2, 3);

        return new KeyPad(keys);
    }

    private static KeyPad GetDirectionalPad()
    {
        var keys = new Dictionary<char, Position>();

        keys[' '] = new Position(0, 0);
        keys['^'] = new Position(1, 0);
        keys['A'] = new Position(2, 0);

        keys['<'] = new Position(0, 1);
        keys['v'] = new Position(1, 1);
        keys['>'] = new Position(2, 1);

        return new KeyPad(keys);
    }

    private static void ReverseProcess(string prefix, string input, int depth)
    {
        for (var i = 0; i < depth; i++)
        {
            input = ExecuteOnDirectionalPad(input);
        }

        input = ExecuteOnNumericPad(input);

        Console.WriteLine($"{prefix} [{input}] Code: {input}");
    }

}

internal record KeyPad(Dictionary<char, Position> Keys);