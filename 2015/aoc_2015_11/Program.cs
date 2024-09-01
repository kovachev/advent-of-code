namespace aoc_2015_11;

internal class Program
{
    internal static void Main(string[] args)
    {
        Console.WriteLine("Advent of Code 2015 - 11");
        
        // Part 1
        var input = "vzbxkghb".ToCharArray();

        input = NextPassword(input);
       
        Console.WriteLine("Part 1: The next valid password is " + new string(input));
        
        // Part 2
        input = NextPassword(input);
        
        Console.WriteLine("Part 2: The next valid password is " + new string(input));
    }

    private static char[] NextPassword(char[] input)
    {
        var count = 0;
        var found = false;
        while (count++ < 1000000)
        {
            input = IncrementPassword(input);
            if (IsValidPassword(input))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            throw new Exception("No valid password found");
        }
        
        return input;
    }
    
    private static char[] IncrementPassword(char[] password)
    {
        for (var i = password.Length - 1; i >= 0; i--)
        {
            if (password[i] == 'z')
            {
                password[i] = 'a';
            }
            else
            {
                password[i]++;
                break;
            }
        }
        
        return password;
    }
    
    private static bool IsValidPassword(char[] password)
    {
        var hasStraight = false;
        
        for (var i = 0; i < password.Length; i++)
        {
            if (password[i] == 'i' || 
                password[i] == 'o' || 
                password[i] == 'l')
            {
                return false;
            }
            
            if (i < password.Length - 2)
            {
                if (password[i] == password[i + 1] - 1 &&
                    password[i] == password[i + 2] - 2)
                {
                    hasStraight = true;
                }
            }
        }
        
        var pairCount = 0;
        for (var i = 0; i < password.Length - 1; i++)
        {
            if (password[i] == password[i + 1])
            {
                pairCount++;
                i++;
            }
        }
        
        return hasStraight && pairCount >= 2;
    }
}