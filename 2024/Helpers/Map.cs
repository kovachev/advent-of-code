using System.Collections;

namespace Helpers;

public class Map: IEnumerable<(Position Position, char Value)>
{
    private readonly char[][] _map;

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

    public bool IsOnMap(Position position)
    {
        return position.X >= 0 && position.X < XMax && position.Y >= 0 && position.Y < YMax;
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
}