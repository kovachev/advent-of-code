namespace aoc_2015_03;

internal class Santa
{
    public HashSet<(int x, int y)> Visited { get; } = [];

    private int _x;
    private int _y;

    public Santa()
    {
        _x = 0;
        _y = 0;
        
        Visited.Add((_x, _y));
    }
    
    public void Move(char directionCharacter)
    {
        switch (directionCharacter)
        {
            case '^':
                _y++;
                break;
            
            case 'v':
                _y--;
                break;
            
            case '>':
                _x++;
                break;
            
            case '<':
                _x--;
                break;
        }
        
        Visited.Add((_x, _y));
    }
    
    public int VisitedHousesCount() => Visited.Count;
    
    public int VisitedHousesCount(HashSet<(int x, int y)> exclude) => Visited.Count(x => !exclude.Contains(x));
}