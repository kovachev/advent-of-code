namespace aoc_2015_21;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 21");
        
        var weapons = new[]
        {
            new Item("Dagger", 8, 4, 0),
            new Item("Shortsword", 10, 5, 0),
            new Item("Warhammer", 25, 6, 0),
            new Item("Longsword", 40, 7, 0),
            new Item("Greataxe", 74, 8, 0)
        };
        
        var armors = new[]
        {
            new Item("None", 0, 0, 0),
            new Item("Leather", 13, 0, 1),
            new Item("Chainmail", 31, 0, 2),
            new Item("Splintmail", 53, 0, 3),
            new Item("Bandedmail", 75, 0, 4),
            new Item("Platemail", 102, 0, 5)
        };
        
        var rings = new[]
        {
            new Item("Damage 0", 0, 0, 0),
            new Item("Damage +1", 25, 1, 0),
            new Item("Damage +2", 50, 2, 0),
            new Item("Damage +3", 100, 3, 0),
            new Item("Defense 0", 0, 0, 0),
            new Item("Defense +1", 20, 0, 1),
            new Item("Defense +2", 40, 0, 2),
            new Item("Defense +3", 80, 0, 3)
        };
        
        var player = new Player(100, 0, 0);
        
        var boss = new Player(109, 8, 2);
        
        var (minCost, maxCost) = Compute(player, boss, weapons, armors, rings);
        
        Console.WriteLine($"Part 1 (min cost to win): {minCost}");
        Console.WriteLine($"Part 2 (max cost and still loose): {maxCost}");
    }

    private static (int MinCostToWin, int MaxCostToLoose) Compute(
        Player player, 
        Player boss, 
        Item[] weapons, 
        Item[] armors,
        Item[] rings)
    {
        var minCostToWin = int.MaxValue;
        var maxCostToLoose = int.MinValue;
        
        foreach (var weapon in weapons)
        {
            player.Equip(weapon);
            
            foreach (var armor in armors)
            {
                player.Equip(armor);
                
                for (var i = 0; i < rings.Length; i++)
                {
                    player.Equip(rings[i]);
                    
                    for (var j = i + 1; j < rings.Length; j++)
                    {
                        player.Equip(rings[j]);
                        
                        if (PlayerWins(player, boss))
                        {
                            if (player.TotalCost < minCostToWin)
                            {
                                minCostToWin = player.TotalCost;
                            }
                        }
                        else
                        {
                            if (player.TotalCost > maxCostToLoose)
                            {
                                maxCostToLoose = player.TotalCost;
                            }
                        }
                        
                        player.UnEquip(rings[j]);
                    }
                    
                    player.UnEquip(rings[i]);
                }
                
                player.UnEquip(armor);
            }
            
            player.UnEquip(weapon);
        }

        return (minCostToWin, maxCostToLoose);
    }

    private static bool PlayerWins(Player player, Player boss)
    {
        var playerDamage = Math.Max(player.TotalDamage - boss.TotalArmor, 1);
        var bossDamage = Math.Max(boss.TotalDamage - player.TotalArmor, 1);
        
        var playerTurns = (int)Math.Ceiling((double)boss.HitPoints / playerDamage);
        var bossTurns = (int)Math.Ceiling((double)player.HitPoints / bossDamage);
        
        return playerTurns <= bossTurns;
    }
}

internal record Player(int HitPoints, int Damage, int Armor)
{
    private readonly ICollection<Item> _inventory = new List<Item>();
    
    public int TotalDamage => Damage + _inventory.Sum(i => i.Damage);
    
    public int TotalArmor => Armor + _inventory.Sum(i => i.Armor);
    
    public int TotalCost => _inventory.Sum(i => i.Cost);
    
    public void Equip(Item item)
    {
        _inventory.Add(item);
    }
    
    public void UnEquip(Item item)
    {
        _inventory.Remove(item);
    }
};

// Code looks prettier with the unused Name property 
// ReSharper disable once NotAccessedPositionalProperty.Global
internal record Item(string Name, int Cost, int Damage, int Armor);