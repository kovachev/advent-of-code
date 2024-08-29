namespace aoc_2015_02;

internal class Box
{
    private readonly int _length;
    private readonly int _width;
    private readonly int _height;
    
    internal Box(string dimensions)
    {
        var parts = dimensions.Split('x');
        _length = int.Parse(parts[0]);
        _width = int.Parse(parts[1]);
        _height = int.Parse(parts[2]);
    }
    
    internal int SurfaceArea => 2 * _length * _width + 2 * _width * _height + 2 * _height * _length;
    
    internal int Volume => _length * _width * _height;
    
    internal int SmallestSideArea => Math.Min(_length * _width, Math.Min(_width * _height, _height * _length));
    
    internal int SmallestCircumference => 2 * Math.Min(_length + _width, Math.Min(_width + _height, _height + _length));
}