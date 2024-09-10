namespace aoc_2015_22;

internal class Program
{
    internal static void Main()
    {
        Console.WriteLine("Advent of Code 2015 - 22");
        
        var minManaSpent = BinarySearch(manaLimit =>
            Recurse(new GameState
            {
                ManaLimit = manaLimit, 
                IsHardMode = false, 
                Debug = false
            })
        );
        
        Console.WriteLine($"Part 1 (min mana spent to win): {minManaSpent}");
    }

    private static bool Recurse(GameState gameState)
    {
        gameState = gameState.DamagePlayer(1);
        gameState = gameState.ApplyEffects();

        foreach (var stateAfterPlayerTurn in gameState.PlayerTurns())
        {
            gameState = stateAfterPlayerTurn.ApplyEffects();
            gameState = gameState.BossTurn();
            
            if (gameState.Boss.HitPoints <= 0 ||
                gameState.Player.HitPoints > 0 && Recurse(gameState))
            {
                return true;
            }
        }

        return false;
    }

    private static int BinarySearch(Func<int, bool> f)
    {
        var hi = 1;
        
        while (!f(hi))
        {
            hi *= 2;
            PrintRed($"Trying with mana limit: {hi}");
        }

        var lo = hi / 2;
     
        
        while (hi - lo > 1)
        {
            var m = (hi + lo) / 2;
            
            PrintRed($"Trying with mana limit: {m} ({lo} - {hi})");
            
            if (f(m))
            {
                hi = m;
            }
            else
            {
                lo = m;
            }
        }

        return hi;
    }

    private static void PrintRed(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}