namespace aoc_2015_22;

internal class GameState
{
    public Player Player { get; set; } = new(hitPoints: 50, mana: 500, manaSpent: 0);

    public Boss Boss { get; set; } = new(hitPoints: 58, damage: 9);

    public int ManaLimit { get; set; } = 500;
    
    public GameState Clone()
    {
        var clone = new GameState
        {
            Player = Player.Clone(),
            Boss = Boss.Clone(),
            ManaLimit = ManaLimit
        };

        return clone;
    }

    public GameState ApplyEffects()
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            return this;
        }

        var newState = Clone();
        
        newState.Boss.HitPoints -= newState.Player.ActiveSpells.Sum(spell => spell.Damage);

        newState.Player.Mana += newState.Player.ActiveSpells.Sum(spell => spell.Mana);
        
        foreach (var activeSpell in newState.Player.ActiveSpells)
        {
            activeSpell.Turns--;
        }

        newState.Player.ActiveSpells = newState.Player
                                               .ActiveSpells
                                               .Where(x => x.Turns > 0)
                                               .ToList();

        return newState;
    }
    
    public GameState HitPlayer(int damage)
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            return this;
        }

        var newState = Clone();
     
        newState.Player.HitPoints -= damage;

        return newState;
    }

    public GameState BossTurn()
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            return this;
        }

        Console.WriteLine("-- Boss turn --");
        Print();
        
        var newState = Clone();
     
        var playerArmor = newState.Player.ActiveSpells.Sum(spell => spell.Armor);
        
        var totalDamage = Math.Max(1, newState.Boss.Damage - playerArmor);

        Console.WriteLine($"Boss attacks for {totalDamage} damage.");
        
        newState.Player.HitPoints -= totalDamage;

        return newState;
    }

    public IEnumerable<GameState> PlayerTurns()
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            yield return this;
            yield break;
        }
        
        Console.WriteLine("-- Player turn --");
        Print();

        if (CanCastSpell(SpellName.MagicMissile))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.MagicMissile);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Boss.HitPoints -= spell.Damage;
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Drain))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Drain);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Boss.HitPoints -= spell.Damage;
            clone.Player.HitPoints += spell.Heal;
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Shield))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Shield);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.ActiveSpells.Add(spell);
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Poison))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Poison);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.ActiveSpells.Add(spell);
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Recharge))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Recharge);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.ActiveSpells.Add(spell);
            yield return clone;
        }
    }
    
    private bool CanCastSpell(string spellName)
    {
        var spell = Spell.GetSpell(spellName, clone: false);
        
        return Player.Mana >= spell.Cost && 
               (Player.ManaSpent + spell.Cost) <= ManaLimit && 
               Player.ActiveSpells.All(x => x.Name != spellName);
    }
    
    private void Print()
    {
        var playerArmor = Player.ActiveSpells.Sum(spell => spell.Armor);
        Console.WriteLine($"- Player has {Player.HitPoints} hit points, {playerArmor} armor, {Player.Mana} mana");
        Console.WriteLine($"- Boss has {Boss.HitPoints} hit points");
    }
}