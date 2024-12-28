using System.Text;
using Helpers;

namespace aoc_2024_21;

public class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 21");

        Process("Part 1", "sample.txt", 2);

        Process("Part 1", "input.txt", 2);
    }

    private static void Process(string prefix, string inputFile, int depth)
    {
        var lines = File.ReadAllLines(inputFile);

        var result = 0L;
        
        foreach (var line in lines)
        {
            var moves = DigitalToDirectional(line);
            
            for (var i = 0; i < depth; i++)
            {
                moves = DirectionalToDirectional(moves);
            }

            var code = int.Parse(line[..^1]);
            var complexity = moves.Length * code;
            
            result += complexity;
            
            Console.WriteLine($"{line} Moves: {moves} Complexity: {complexity} = {moves.Length} * {code}");
        }
        
        // <vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A
        // v<A<AA>^>AvA^<Av>A^Av<<A>^>AvA^Av<<A>^>AAv<A>A^A<A>Av<A<A>^>AAA<Av>A^A 
        
        Console.WriteLine($"{prefix} [{inputFile}] Total Complexity: {result}");
    }

    private static string DigitalToDirectional(string input)
    {
        var numericPad = GetNumericPad();
        
        var moves = new StringBuilder();
            
        foreach (var ch in input.AsSpan())
        {
            var position = numericPad.inputs[ch];
               
            if (position == numericPad.Position)
            {
                moves.Append('A');
                continue;
            }
                
            var result = numericPad.map.FindPath(numericPad.Position, position);
            if (result == null)
            {
                throw new Exception("Having a bad day");
            }

            numericPad = numericPad with { Position = position };
                
            var path = result.Path;
            for (var i = 0; i < path.Length - 1; i++)
            {
                var from = path[i];
                var to = path[i + 1];

                if (from.X < to.X)
                {
                    moves.Append('>');
                }
                else if (from.X > to.X)
                {
                    moves.Append('<');
                }
                else if (from.Y < to.Y)
                {
                    moves.Append('v');
                }
                else if (from.Y > to.Y)
                {
                    moves.Append('^');
                }
            }
                
            moves.Append('A');
        }

        return moves.ToString();
    }

    private static string DirectionalToDirectional(string input)
    {
        var directionalPad = GetDirectionalPad();
        
        var moves = new StringBuilder();
        
        foreach (var ch in input.AsSpan())
        {
            var position = directionalPad.inputs[ch];
            
            if (position == directionalPad.Position)
            {
                moves.Append('A');
                continue;
            }
            
            var result = directionalPad.map.FindPath(directionalPad.Position, position);
            if (result == null)
            {
                throw new Exception("Having a bad day");
            }

            directionalPad = directionalPad with { Position = position };
            
            var path = result.Path;
            for (var i = 0; i < path.Length - 1; i++)
            {
                var from = path[i];
                var to = path[i + 1];

                if (from.X < to.X)
                {
                    moves.Append('>');
                }
                else if (from.X > to.X)
                {
                    moves.Append('<');
                }
                else if (from.Y < to.Y)
                {
                    moves.Append('v');
                }
                else if (from.Y > to.Y)
                {
                    moves.Append('^');
                }
            }
            
            moves.Append('A');
        }
        
        return moves.ToString();
    }
    
    private static Pad GetNumericPad()
    {
        var map = new Map(3, 4);
        map[0, 3] = '#';

        var digits = new Dictionary<char, Position>();
        digits['7'] = new Position(0, 0);
        digits['8'] = new Position(1, 0);
        digits['9'] = new Position(2, 0);
        digits['4'] = new Position(0, 1);
        digits['5'] = new Position(1, 1);
        digits['6'] = new Position(2, 1);
        digits['1'] = new Position(0, 2);
        digits['2'] = new Position(1, 2);
        digits['3'] = new Position(2, 2);
        digits['0'] = new Position(1, 3);
        digits['A'] = new Position(2, 3);

        return new Pad(map, new Position(2, 3), digits);
    }

    private static Pad GetDirectionalPad()
    {
        var map = new Map(3, 2);
        map[0, 0] = '#';

        var directions = new Dictionary<char, Position>();
        directions['^'] = new Position(1, 0);
        directions['A'] = new Position(2, 0);
        directions['<'] = new Position(0, 1);
        directions['v'] = new Position(1, 1);
        directions['>'] = new Position(2, 1);

        return new Pad(map, new Position(2, 0), directions);
    }
}

internal record Pad(Map map, Position Position, Dictionary<char, Position> inputs);