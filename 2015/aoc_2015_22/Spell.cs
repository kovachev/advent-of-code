namespace aoc_2015_22;

internal struct SpellName
{
    public const string MagicMissile = "Magic Missile";
    public const string Drain = "Drain";
    public const string Shield = "Shield";
    public const string Poison = "Poison";
    public const string Recharge = "Recharge";
}

internal class Spell
{
    public string Name { get; }

    public int Cost { get; }

    public int Damage { get; }

    public int Heal { get; }

    public int Armor { get; }

    public int Mana { get; }

    public int Turns { get; set; }

    public static Spell[] Spells =
    [
        new(name: SpellName.MagicMissile, cost: 53, damage: 4, heal: 0, armor: 0, mana: 0, turns: 0),
        new(name: SpellName.Drain, cost: 73, damage: 2, heal: 2, armor: 0, mana: 0, turns: 0),
        new(name: SpellName.Shield, cost: 113, damage: 0, heal: 0, armor: 7, mana: 0, turns: 6),
        new(name: SpellName.Poison, cost: 173, damage: 3, heal: 0, armor: 0, mana: 0, turns: 6),
        new(name: SpellName.Recharge, cost: 229, damage: 0, heal: 0, armor: 0, mana: 101, turns: 5)
    ];
    
    public static Spell GetSpell(string name, bool clone = true)
    {
        var spell = Spells.Single(x => x.Name == name);

        return clone ? spell.Clone() : spell;
    }
    
    public Spell(
        string name,
        int cost, 
        int damage,
        int heal, 
        int armor,
        int mana,
        int turns)
    {
        Name = name;
        Cost = cost;
        Damage = damage;
        Heal = heal;
        Armor = armor;
        Mana = mana;
        Turns = turns;
    }

    public Spell Clone()
    {
        return new Spell(Name, Cost, Damage, Heal, Armor, Mana, Turns);
    }
}