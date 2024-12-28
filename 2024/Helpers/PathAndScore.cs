namespace Helpers;

public record PathAndScore(Position[] Path, int Score)
{
    public void Deconstruct(out Position[] path, out int score)
    {
        path = Path;
        score = Score;
    }
}