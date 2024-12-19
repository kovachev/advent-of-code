using Helpers;

namespace aoc_2024_14;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 14");
        
        // var map = new Map(11, 7);
        // var input = File.ReadAllLines("sample.txt");
        
        var map = new Map(101, 103);
        var input = File.ReadAllLines("input.txt");
        
        var robots = input.Select(ParseRobot)
                          .ToArray();

        Part1(map, robots);
        
        Console.WriteLine();
        Console.WriteLine();
        
        robots = input.Select(ParseRobot)
                      .ToArray();
        
        Part2(map, robots);
    }

    private static void Part1(Map map, Robot[] robots)
    {
        for (var i = 0; i < 100; i++)
        {
            Tick(map, robots);
        }
        
        // foreach (var position in map)
        // {
        //     var count = robots.Count(r => r.Position == position.Position);
        //     map[position.Position] = count == 0 ? '.' : count.ToString()[0];
        // }
        //
        // map.Print();
        
        var xhalf = map.XMax / 2;
        var yhalf = map.YMax / 2;
        
        var quadrant1 = robots.Where(r => r.Position.X >= 0 && r.Position.Y >= 0 && 
                                          r.Position.X < xhalf && r.Position.Y < yhalf)
                              .ToArray();
        
        var quadrant2 = robots.Where(r => r.Position.X > xhalf && r.Position.Y >= 0 &&
                                          r.Position.Y < yhalf)
                              .ToArray();
        
        var quadrant3 = robots.Where(r => r.Position.X >= 0 && r.Position.Y > yhalf &&
                                          r.Position.X < xhalf)
                              .ToArray();
        
        var quadrant4 = robots.Where(r => r.Position.X > xhalf && r.Position.Y > yhalf)
                              .ToArray();
        
        var part1 = quadrant1.Length * quadrant2.Length * quadrant3.Length * quadrant4.Length;
        
        Console.WriteLine($"Part 1: {part1}");
    }
    
    private static void Part2(Map map, Robot[] robots)
    {
        var seconds = 0;
        
        while (true)
        {
            seconds++;
            
            Tick(map, robots);
            
            var mapCopy = map.Clone();
            foreach (var robot in robots)
            {
                mapCopy[robot.Position] = 'R';
            }

            for (var y = 0; y < mapCopy.YMax; y++)
            {
                var row = new string(mapCopy.GetRow(y));
                if (row.Contains("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR"))
                {
                    mapCopy.Print();
                    Console.WriteLine($"Part 2: {seconds}");
                    return;
                }
            }
        }
    }
    
    private static void Tick(Map map, Robot[] robots)
    {
        for (var r = 0; r < robots.Length; r++)
        {
            var robot = robots[r];
            
            var x = (robot.Position.X + robot.Velocity.X + map.XMax ) % map.XMax;
            var y = (robot.Position.Y + robot.Velocity.Y + map.YMax ) % map.YMax;
            
            robots[r] = robot with { Position = new Position(x, y) };
        }
    }
    
    private static Robot ParseRobot(string line)
    {
        var parts = line.Split(" ");
        
        var positionParts = parts[0][2..].Split(',');
        var position = new Position(int.Parse(positionParts[0]), int.Parse(positionParts[1]));
        
        var velocityParts = parts[1][2..].Split(',');
        var velocity = new Position(int.Parse(velocityParts[0]), int.Parse(velocityParts[1]));
        
        return new Robot(position, velocity);
    }
}

internal record Robot(Position Position, Position Velocity);