namespace aoc_2015_22;

internal class Boss
{
    public int HitPoints { get; set; }

    public int Damage { get; }

    public Boss(int hitPoints, int damage)
    {
        HitPoints = hitPoints;
        Damage = damage;
    }
    
    public Boss Clone()
    {
        return new Boss(HitPoints, Damage);
    }
}