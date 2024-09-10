namespace aoc_2015_22;

internal class GameState
{
    public Player Player { get; set; } = new(hitPoints: 50, mana: 500, manaSpent: 0);

    public Boss Boss { get; set; } = new(hitPoints: 58, damage: 9);

    public int ManaLimit { get; set; } = 500;

    public int Depth { get; set; }
    
    public bool IsHardMode { get; set; }
    
    public bool Debug { get; set; }
    
    public GameState Clone()
    {
        var clone = new GameState
        {
            Player = Player.Clone(),
            Boss = Boss.Clone(),
            ManaLimit = ManaLimit,
            Depth = Depth + 1,
            IsHardMode = IsHardMode,
            Debug = Debug
        };

        return clone;
    }
    
    public GameState DamagePlayer(int damage)
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            PrintSpells();
            return this;
        }
        
        var newState = Clone();
        newState.Player.HitPoints -= damage;
        return newState;
    }

    public GameState ApplyEffects()
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            PrintSpells();
            return this;
        }

        var newState = Clone();
        
        newState.Boss.HitPoints -= newState.Player.ActiveSpells.Sum(spell => spell.Damage);

        newState.Player.Mana += newState.Player.ActiveSpells.Sum(spell => spell.Mana);
        
        foreach (var activeSpell in newState.Player.ActiveSpells)
        {
            activeSpell.Turns--;

            if (Debug)
            {
                switch (activeSpell.Name)
                {
                    case SpellName.Shield:
                        Console.WriteLine($"[{Depth}] {activeSpell.Name} adds {activeSpell.Armor} armor; its timer is now {activeSpell.Turns}.");
                        break;

                    case SpellName.Poison:
                        Console.WriteLine($"[{Depth}] {activeSpell.Name} deals {activeSpell.Damage} damage; its timer is now {activeSpell.Turns}.");
                        break;

                    case SpellName.Recharge:
                        Console.WriteLine($"[{Depth}] {activeSpell.Name} provides {activeSpell.Mana} mana; its timer is now {activeSpell.Turns}.");
                        break;
                }
            }
        }

        newState.Player.ActiveSpells = newState.Player
                                               .ActiveSpells
                                               .Where(x => x.Turns > 0)
                                               .ToList();

        return newState;
    }

    public GameState BossTurn()
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            PrintSpells();
            return this;
        }

        if (Debug) Console.WriteLine($"[{Depth}] -- Boss turn --");
        Print();
        
        var newState = Clone();
     
        var playerArmor = newState.Player.ActiveSpells.Sum(spell => spell.Armor);
        
        var totalDamage = Math.Max(1, newState.Boss.Damage - playerArmor);

        if (Debug) Console.WriteLine($"[{Depth}] Boss attacks for {totalDamage} damage.");
        
        newState.Player.HitPoints -= totalDamage;

        return newState;
    }

    public IEnumerable<GameState> PlayerTurns()
    {
        if (Player.HitPoints <= 0 || Boss.HitPoints <= 0)
        {
            PrintSpells();
            yield return this;
            yield break;
        }
        
        if (Debug) Console.WriteLine($"[{Depth}] -- Player turn --");
        Print();

        if (CanCastSpell(SpellName.MagicMissile))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.MagicMissile);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.SpellsHistory.Add(spell.Name);
            clone.Boss.HitPoints -= spell.Damage;
            
            if (Debug) Console.WriteLine($"[{Depth}] Player casts Magic Missile, dealing 4 damage.");
            
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Drain))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Drain);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.HitPoints += spell.Heal;
            clone.Player.SpellsHistory.Add(spell.Name);
            clone.Boss.HitPoints -= spell.Damage;
            
            if (Debug) Console.WriteLine($"[{Depth}] Player casts Drain, dealing 2 damage and healing 2 hit points.");
            
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Shield))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Shield);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.ActiveSpells.Add(spell);
            clone.Player.SpellsHistory.Add(spell.Name);
            
            if (Debug) Console.WriteLine($"[{Depth}] Player casts Shield.");
            
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Poison))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Poison);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.ActiveSpells.Add(spell);
            clone.Player.SpellsHistory.Add(spell.Name);
            
            if (Debug) Console.WriteLine($"[{Depth}] Player casts Poison.");
            
            yield return clone;
        }
        
        if (CanCastSpell(SpellName.Recharge))
        {
            var clone = Clone();
            var spell = Spell.GetSpell(SpellName.Recharge);
            clone.Player.Mana -= spell.Cost;
            clone.Player.ManaSpent += spell.Cost;
            clone.Player.ActiveSpells.Add(spell);
            clone.Player.SpellsHistory.Add(spell.Name);
            
            if (Debug) Console.WriteLine($"[{Depth}] Player casts Recharge.");
            
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
        if (!Debug)
        {
            return;
        }
        
        var playerArmor = Player.ActiveSpells.Sum(spell => spell.Armor);
        Console.WriteLine($"[{Depth}] - Player has {Player.HitPoints} hit points, {playerArmor} armor, {Player.Mana} mana, {Player.ManaSpent} mana spent");
        Console.WriteLine($"[{Depth}] - Boss has {Boss.HitPoints} hit points");
    }
    
    private void PrintSpells()
    {
        if (!Debug)
        {
            return;
        }
        
        if (Player.HitPoints <= 0)
        {
            Console.WriteLine($"[{Depth}] --- Player died");
            return;
        }
        
        Console.WriteLine($"[{Depth}] --- Player used {Player.SpellsHistory.Count} spells:");
        foreach (var spellName in Player.SpellsHistory)
        {
            Console.WriteLine($"[{Depth}]   - {spellName}");
        }
    }
}