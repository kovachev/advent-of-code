using Helpers;

namespace aoc_2024_06;

internal class Program
{
    private const char Empty = '.';
    private const char VisitedCross = '+';
    private const char VisitedVertical = '|';
    private const char VisitedHorizontal = '-';
    private const char Wall = '#';
    private const char Start = '^';
    
    private static Map _initialMap = new();
    private static Position[] _wallPositions = [];
    private static Position _startPosition = new (0, 0);
    private static readonly HashSet<Position> _loopWalls = [];
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 6");
        
        _initialMap = new Map("input.txt");
        //_initialMap = new Map("sample.txt");
        
        _startPosition = _initialMap.Single(x => x.Value == Start).Position;
        _wallPositions = _initialMap.Where(x => x.Value == Wall).Select(x => x.Position).ToArray();
        
        var pathLength = TraverseMap(_initialMap.Clone(), out var visited);
        FindLoops(_initialMap.Clone(), visited);

        Console.WriteLine($"Start Position: {_startPosition}");
        
        Console.WriteLine($"Part 1 (Path Length): {pathLength}");
        Console.WriteLine($"Part 2 (Loops): {_loopWalls.Count}");
    }
    
    private static void PrintMap(Map map, Position[] loopWalls)
    {
        Console.Write("    ");
        for (var x = 0; x < map.XMax; x++)
        {
            Console.Write(x % 10);
        }
        Console.WriteLine();
        
        Console.Write("    ");
        for (var x = 0; x < map.XMax; x++)
        {
            Console.Write('-');
        }
        Console.WriteLine();
        
        for (var y = 0; y < map.YMax; y++)
        {
            Console.Write(y.ToString().PadLeft(3, ' ') + "|");
            for (var x = 0; x < map.XMax; x++)
            {
                if (loopWalls.Any(wall => wall == new Position(x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("O");
                }
                else
                {
                    Console.ResetColor();
                    Console.Write(map[x,y]);
                }
            }
            
            Console.WriteLine();
        }
    }

    private static int TraverseMap(Map map, out Dictionary<Position, List<Direction>> visited, bool display = false)
    {
        var position = _startPosition;
        var direction = Direction.Up;
        
        visited = new Dictionary<Position, List<Direction>>();
        visited[position] = [direction];
        
        map[position] = VisitedVertical;
        
        var pathLength = 1; // Start position is already visited
        
        while (true)
        {
            var next = position.Move(direction.ToPosition());
            if (!map.IsOnMap(next))
            {
                break;
            }

            if (map[next] == Wall)
            {
                direction = direction.TurnRight();
                map[position] = VisitedCross;
            }
            else
            {
                switch (map[next])
                {
                    case Empty:
                        map[next] = direction.IsHorizontal() ? VisitedHorizontal : VisitedVertical;
                        pathLength++;
                        break;

                    case VisitedVertical:
                    case VisitedHorizontal:
                        map[next] = VisitedCross;
                        break;
                }

                position = next;
            }

            if (!visited.TryAdd(position, [direction]))
            {
                if (visited[position].Contains(direction))
                {
                    return 0;
                }
                
                visited[position].Add(direction);
            }
            
            if (display)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                PrintMap(map, []);
                Thread.Sleep(50);
            }
        }
        
        return pathLength;
    }
    
    private static void FindLoops(Map map, Dictionary<Position, List<Direction>> visited)
    {
        var possibleLoopWallPositions = new List<Position>();
        
        foreach (var (position, directions) in visited)
        {
            foreach (var direction in directions)
            {
                if (HasWallDirectlyInFrontOfCurrentLocation(position, direction))
                {
                    continue;
                }
                
                var hasWall = HasWallToTheRightOfCurrentDirection(position, direction);

                if (hasWall)
                {
                    var wallPosition = position.Move(direction.ToPosition());
                    if (map.IsOnMap(wallPosition))
                    {
                        possibleLoopWallPositions.Add(wallPosition);
                    }
                }
            }
        }
        
        // Console.Clear();
        // PrintMap(map, possibleLoopWallPositions.ToArray());
        //
        // return;
        
        for (var i = 0; i < possibleLoopWallPositions.Count; i++)
        {
            Console.WriteLine($"Traversing loop candidate {i+1} of {possibleLoopWallPositions.Count}");
            
            var position = possibleLoopWallPositions[i];
            var mapCopy = map.Clone();
            mapCopy[position.X, position.Y] = Wall;
            
            var pathLength = TraverseMap(mapCopy, out _);
            if (pathLength == 0)
            {
                _loopWalls.Add(position);
            }
        }
        
        Console.Clear();
        PrintMap(map, _loopWalls.ToArray());
    }
    
    private static bool HasWallDirectlyInFrontOfCurrentLocation(Position position, Direction currentDirection)
    {
        var next = position.Move(currentDirection.ToPosition());

        return _wallPositions.Any(wall => wall == next);
    }
    
    private static bool HasWallToTheRightOfCurrentDirection(Position position, Direction currentDirection)
    {
        return currentDirection switch
        {
            Direction.Up => _wallPositions.Any(wall => wall.Y == position.Y && wall.X > position.X),
            Direction.Down => _wallPositions.Any(wall => wall.Y == position.Y && wall.X < position.X),
            Direction.Right => _wallPositions.Any(wall => wall.X == position.X && wall.Y > position.Y),
            Direction.Left => _wallPositions.Any(wall => wall.X == position.X && wall.Y < position.Y),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}