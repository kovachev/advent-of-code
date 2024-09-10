namespace aoc_2015_22;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 22");
        
        var minManaSpent = int.MaxValue;
        
        for (var manaLimit = 53; manaLimit < 100000; manaLimit++)
        {
            var gameState = new GameState { ManaLimit = manaLimit };
            
            if (Recurse(gameState))
            {
                minManaSpent = Math.Min(minManaSpent, gameState.Player.ManaSpent);
            }
        }
        
        Console.WriteLine($"Part 1 (min mana spent to win): {minManaSpent}");
    }

    private static bool Recurse(GameState gameState)
    {
        gameState = gameState.ApplyEffects();
     
        foreach (var stateT in gameState.PlayerTurns())
        {
            gameState = stateT.ApplyEffects();
            gameState = gameState.BossTurn();
            if (gameState.Boss.HitPoints <= 0 || 
                gameState.Player.HitPoints > 0 && Recurse(gameState))
            {
                return true;
            }
        }

        return false;
    }
}