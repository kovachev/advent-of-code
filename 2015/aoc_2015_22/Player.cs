﻿namespace aoc_2015_22;

internal class Player
{
    public int HitPoints { get; set; }

    public int ManaSpent { get; set; }
    
    public int Mana { get; set; }

    public IList<Spell> ActiveSpells { get; set; } = new List<Spell>();

    public Player(int hitPoints, int mana, int manaSpent)
    {
        HitPoints = hitPoints;
        Mana = mana;
        ManaSpent = manaSpent;
    }

    public Player Clone()
    {
        var clone = new Player(HitPoints, Mana, ManaSpent);
        
        foreach (var activeSpell in ActiveSpells)
        {
            clone.ActiveSpells.Add(activeSpell.Clone());
        }
        
        return clone;
    }
}