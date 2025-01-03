using System.Collections;

namespace Helpers;

public class Map: IEnumerable<(Position Position, char Value)>
{
    public const char WallMarker = '#';
    public const char StartMarker = 'S';
    public const char EndMarker = 'E';
    public const char EmptyMarker = '.';
    
    private readonly char[][] _map;

    public Map()
    {
        _map = [];
    }
    
    public Map(string inputFile)
    {
        _map = File.ReadAllLines(inputFile).Select(x => x.ToCharArray()).ToArray();
    }
    
    public Map(string[] map)
    {
        _map = map.Select(x => x.ToCharArray()).ToArray();
    }
    
    public Map(char[][] map)
    {
        _map = map;
    }
    
    public Map(int xMax, int yMax, char defaultValue = EmptyMarker)
    {
        _map = new char[yMax][];
        for (var y = 0; y < yMax; y++)
        {
            _map[y] = new char[xMax];
            for (var x = 0; x < xMax; x++)
            {
                _map[y][x] = defaultValue;
            }
        }
    }

    public char this[int x, int y] 
    {
        get => _map[y][x];
        set => _map[y][x] = value;
    }
    
    public char this[Position position]
    {
        get => _map[position.Y][position.X];
        set => _map[position.Y][position.X] = value;
    }

    public int XMax => _map[0].Length;
    
    public int YMax => _map.Length;

    public bool TryGetValue(Position position, out char? value)
    {
        if (IsOnMap(position))
        {
            value = this[position];
            return true;
        }

        value = null;
        return false;
    }
    
    public char GetOrDefault(Position position, char defaultValue)
    {
        return IsOnMap(position) ? this[position] : defaultValue;
    }
    
    public char[] GetRow(int y)
    {
        return _map[y];
    }
    
    public bool IsOnMap(Position position)
    {
        return position.X >= 0 && position.X < XMax && position.Y >= 0 && position.Y < YMax;
    }

    public Map Clone()
    {
        return new Map(_map.Select(x => x.ToArray()).ToArray());
    }
    
    public void Print()
    {
        _prevPositions = null;
        
        for (var y = 0; y < YMax; y++)
        {
            for (var x = 0; x < XMax; x++)
            {
                Console.Write(_map[y][x]);
            }
            Console.WriteLine();
        }
    }
    
    public IEnumerable<Position> GetNeighbours(Position position, bool includeDiagonals = false)
    {
        return GetNeighbours(position, _ => true, includeDiagonals);
    }
    
    public IEnumerable<Position> GetNeighbours(Position position, Func<char, bool> condition, bool includeDiagonals = false)
    {
        var up = position + Position.Up;
        if (IsOnMap(up) && condition(this[up]))
        {
            yield return up;
        }
        
        var down = position + Position.Down;
        if (IsOnMap(down) && condition(this[down]))
        {
            yield return down;
        }
        
        var left = position + Position.Left;
        if (IsOnMap(left) && condition(this[left]))
        {
            yield return left;
        }
        
        var right = position + Position.Right;
        if (IsOnMap(right) && condition(this[right]))
        {
            yield return right;
        }
        
        if (includeDiagonals)
        {
            foreach (var neighbour in GetDiagonalNeighbours(position, condition))
            {
                yield return neighbour;
            }
        }
    }

    public IEnumerable<Position> GetDiagonalNeighbours(Position position, Func<char, bool> condition)
    {
        var upLeft = position + Position.UpLeft;
        if (IsOnMap(upLeft) && condition(this[upLeft]))
        {
            yield return upLeft;
        }
            
        var upRight = position + Position.UpRight;
        if (IsOnMap(upRight) && condition(this[upRight]))
        {
            yield return upRight;
        }
            
        var downLeft = position + Position.DownLeft;
        if (IsOnMap(downLeft) && condition(this[downLeft]))
        {
            yield return downLeft;
        }
            
        var downRight = position + Position.DownRight;
        if (IsOnMap(downRight) && condition(this[downRight]))
        {
            yield return downRight;
        }
    }
    
    public IEnumerator<(Position Position, char Value)> GetEnumerator()
    {
        for (var y = 0; y < YMax; y++)
        {
            for (var x = 0; x < XMax; x++)
            {
                yield return (new Position(x, y), _map[y][x]);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Restore(Map mapCopy)
    {
        for (var y = 0; y < YMax; y++)
        {
            for (var x = 0; x < XMax; x++)
            {
                _map[y][x] = mapCopy._map[y][x];
            }
        }
    }

    public void Clear(char value = '.')
    {
        for (var y = 0; y < YMax; y++)
        {
            for (var x = 0; x < XMax; x++)
            {
                _map[y][x] = value;
            }
        }
    }
    
    // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    public PathAndScore? FindPath(Position startPosition, Position endPosition, bool debug = false)
    {
        if (debug)
        {
            Console.Clear();
            Print();
        }
        
        var queue = new PriorityQueue<Position, int>();
        queue.Enqueue(startPosition, 0);

        PathAndScore? result = null;
        
        var visited = new HashSet<Position>();
        foreach (var position in this.Where(x => x.Value == WallMarker).Select(x => x.Position))
        {
            visited.Add(position);
        }

        while (queue.TryDequeue(out var current, out var score))
        {
            visited.Add(current);
            
            foreach (var neighbour in GetNeighbours(current))
            {
                var newScore = score + 1;
                if (result != null && result.Score < newScore)
                {
                    continue;
                }
                
                if (visited.Any(p => p.X == neighbour.X && p.Y == neighbour.Y))
                {
                    continue;
                }
                
                var neighbourWithParent = neighbour with { Parent = current };
                
                if (neighbour == endPosition)
                {
                    var path = neighbourWithParent.ExtractPath(reverse: true).ToArray();
                    
                    if (result == null || result.Score > newScore)
                    {
                        result = new PathAndScore(path, newScore);
                    }
                    
                    continue;
                }
                
                visited.Add(neighbour);
                
                queue.Enqueue(neighbourWithParent, newScore);

                if (debug)
                {
                    var pathWithColor = neighbourWithParent.ExtractPath().Select(p => (p, ConsoleColor.Yellow)).ToList();
                    pathWithColor[0] = (pathWithColor[0].Item1, ConsoleColor.Cyan);
                    ShowPath(pathWithColor);
                    Thread.Sleep(50);
                }
            }
        }
        
        return result;
    }
    
    private IEnumerable<Position>? _prevPositions;

    private void ShowPath(IEnumerable<(Position, ConsoleColor)> positions)
    {
        if (_prevPositions?.Any() == true)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            foreach (var position in _prevPositions)
            {
                Console.SetCursorPosition(position.X, position.Y);
                Console.Write(this[position]);
            }
        }

        foreach (var (position, color) in positions)
        {
            Console.SetCursorPosition(position.X, position.Y);
            Console.BackgroundColor = color;
            Console.Write(this[position]);
        }

        _prevPositions = positions.Select(x => x.Item1);
    }
}