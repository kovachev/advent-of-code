namespace aoc_2024_06;

internal class Program
{
    private const char Empty = '.';
    private const char VisitedCross = '+';
    private const char VisitedVertical = '|';
    private const char VisitedHorizontal = '-';
    private const char Wall = '#';
    private const char Start = '^';
    
    private static char[,] _initialMap = new char[0,0];
    private static Position[] _wallPositions = [];
    private static Position _startPosition = new (0, 0);
    private static readonly HashSet<Position> _loopWalls = [];
    
    private static async Task Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 6");
        
        var input = await File.ReadAllLinesAsync("input.txt");
        //var input = await File.ReadAllLinesAsync("sample.txt");

        LoadMap(input);
        
        _startPosition = FindStartingPoint(_initialMap);
        _wallPositions = FindWalls(_initialMap).ToArray();
        
        var pathLength = TraverseMap(CloneMap(_initialMap), out var visited);
        FindLoops(CloneMap(_initialMap), visited);

        Console.WriteLine($"Start Position: {_startPosition.X},{_startPosition.Y}");
        
        Console.WriteLine($"Part 1 (Path Length): {pathLength}");
        Console.WriteLine($"Part 2 (Loops): {_loopWalls.Count}");
    }

    private static void LoadMap(string[] lines)
    {
        _initialMap = new char[lines[0].Length, lines.Length];
        
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                _initialMap[x,y] = lines[y][x];
            }
        }
    }
    
    private static Position FindStartingPoint(char[,] map)
    {
        for (var y = 0; y < map.GetLength(1); y++)
        {
            for (var x = 0; x < map.GetLength(0); x++)
            {
                if (map[x,y] == Start)
                {
                    return new Position(x, y);
                }
            }
        }
        
        return new Position(-1, -1);
    }
    
    private static IEnumerable<Position> FindWalls(char[,] map)
    {
        for (var y = 0; y < map.GetLength(1); y++)
        {
            for (var x = 0; x < map.GetLength(0); x++)
            {
                if (map[x,y] == Wall)
                {
                    yield return new Position(x, y);
                }
            }
        }
    }
    
    private static void PrintMap(char[,] map, Position[] loopWalls)
    {
        Console.Write("    ");
        for (var x = 0; x < map.GetLength(0); x++)
        {
            Console.Write(x % 10);
        }
        Console.WriteLine();
        
        Console.Write("    ");
        for (var x = 0; x < map.GetLength(0); x++)
        {
            Console.Write('-');
        }
        Console.WriteLine();
        
        for (var y = 0; y < map.GetLength(1); y++)
        {
            Console.Write(y.ToString().PadLeft(3, ' ') + "|");
            for (var x = 0; x < map.GetLength(0); x++)
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
    
    private static char[,] CloneMap(char[,] source)
    {
        var target = new char[source.GetLength(0), source.GetLength(1)];
        
        for (var y = 0; y < source.GetLength(1); y++)
        {
            for (var x = 0; x < source.GetLength(0); x++)
            {
                target[x,y] = source[x,y];
            }
        }
        
        return target;
    }

    private static int TraverseMap(char[,] map, out Dictionary<Position, List<Direction>> visited, bool display = false)
    {
        var position = _startPosition;
        var direction = Direction.Up;
        
        visited = new Dictionary<Position, List<Direction>>();
        visited[position] = [direction];
        
        map[position.X, position.Y] = VisitedVertical;
        
        var pathLength = 1; // Start position is already visited
        
        while (true)
        {
            var next = position.Move(direction);
            if (!next.IsOnMap(map))
            {
                break;
            }

            if (map[next.X, next.Y] == Wall)
            {
                direction = direction.TurnRight();
                map[position.X, position.Y] = VisitedCross;
            }
            else
            {
                switch (map[next.X, next.Y])
                {
                    case Empty:
                        map[next.X, next.Y] = direction.IsHorizontal() ? VisitedHorizontal : VisitedVertical;
                        pathLength++;
                        break;

                    case VisitedVertical:
                    case VisitedHorizontal:
                        map[next.X, next.Y] = VisitedCross;
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
    
    private static void FindLoops(char[,] map, Dictionary<Position, List<Direction>> visited)
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
                    var wallPosition = position.Move(direction);
                    if (wallPosition.IsOnMap(map))
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
            var mapCopy = CloneMap(map);
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
        var next = position.Move(currentDirection);

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